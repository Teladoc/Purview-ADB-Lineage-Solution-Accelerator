using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Function.Domain.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Function.Domain.Models.OL;
using System.Linq;
using Function.Domain.Helpers.Logging;
using Function.Domain.Models.Messaging;
using Microsoft.Azure.Amqp;
using System.Diagnostics;

namespace AdbToPurview.Function
{
    public class PurviewOut
    {
        private readonly ILogger<PurviewOut> _logger;
        private readonly IOlConsolidateEnrichFactory _olEnrichmentFactory;
        private readonly IOlToPurviewParsingService _olToPurviewParsingService;
        private readonly IPurviewIngestion _purviewIngestion;
        private readonly IOlClaimCheckService _olClaimCheckService;
        private readonly IOlFilter _olFilter;

        public PurviewOut(ILogger<PurviewOut> logger,
            IOlToPurviewParsingService olToPurviewParsingService,
            IPurviewIngestion purviewIngestion,
            IOlConsolidateEnrichFactory olEnrichmentFactory,
            IOlClaimCheckService olClaimCheckService,
            IOlFilter olFilter)
        {
            logger.LogInformation("Enter PurviewOut");
            _logger = logger;
            _olEnrichmentFactory = olEnrichmentFactory;
            _olToPurviewParsingService = olToPurviewParsingService;
            _purviewIngestion = purviewIngestion;
            _olClaimCheckService = olClaimCheckService ?? throw new ArgumentNullException(nameof(olClaimCheckService));
            _olFilter = olFilter ?? throw new ArgumentNullException(nameof(olFilter));
        }

        [Function("PurviewOut")]
        // V2 May want to implement batch processing of events by making "input" an array and setting IsBatched
        // see https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-hubs-trigger?tabs=csharp#scaling
        public async Task<string> Run(
            [EventHubTrigger("%EventHubName%", IsBatched = false, Connection = "ListenToMessagesFromEventHub", ConsumerGroup = "%EventHubConsumerGroup%")] string input)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            OlClaimCheckMessage? claimCheckMessage = null;
            try
            {
                string? payload;
                if (_olFilter.DoesMessageContainOlSchema(input))
                {
                    // backwards compatible processing
                    payload = input;
                    if (string.IsNullOrWhiteSpace(payload))
                    {                        
                        _logger.LogInformation("Event skipped since payload is empty.");
                        return "Event skipped since payload is empty.";
                    }
                }
                else
                {
                    // Get payload from claim check
                    claimCheckMessage = GetClaimCheckMessage(input);
                    payload = await _olClaimCheckService.GetClaimCheckPayloadAsync(claimCheckMessage.ClaimCheck.Id);
                    if (string.IsNullOrWhiteSpace(payload))
                    {
                        _logger.LogInformation("Event skipped since payload is empty. Used claim check {claimCheck}", claimCheckMessage.ClaimCheck.Id);
                        return "Event skipped since payload is empty.";
                    }
                }
                
                //Check if event is from Azure Synapse Spark Pools                
                if (payload.Contains("azuresynapsespark"))
                {
                    var olSynapseEnrichment = _olEnrichmentFactory.CreateEnrichment<EnrichedSynapseEvent>(OlEnrichmentType.Synapse);
                    var olSynapseEvent = await olSynapseEnrichment.ProcessOlMessage(payload);
                    if (olSynapseEvent == null || olSynapseEvent.OlEvent == null)
                    {
                        _logger.LogInformation("Start event, duplicate event, or no context found for Synapse - eventData: {eventData}", payload);
                        return string.Empty;
                    }

                    _logger.LogInformation($"PurviewOut-ParserService:Processing lineage for Synapse Workspace {olSynapseEvent.OlEvent.Job.Namespace.Split(",").First()}");
                    var purviewSynapseEvent1 = await _olToPurviewParsingService.GetParentEntityAsync(olSynapseEvent);
                    _logger.LogInformation($"PurviewOut-ParserService: {purviewSynapseEvent1}");
                    var jObjectPurviewEvent1 = JsonConvert.DeserializeObject<JObject>(purviewSynapseEvent1!) ?? [];
                    _logger.LogInformation("Calling SendToPurview");
                    await _purviewIngestion.SendToPurview(jObjectPurviewEvent1);

                    var purviewSynapseEvent2 = await _olToPurviewParsingService.GetChildEntityAsync(olSynapseEvent);
                    _logger.LogInformation($"PurviewOut-ParserService: {purviewSynapseEvent2}");
                    var jObjectPurviewEvent2 = JsonConvert.DeserializeObject<JObject>(purviewSynapseEvent2!) ?? [];
                    _logger.LogInformation("Calling SendToPurview");
                    await _purviewIngestion.SendToPurview(jObjectPurviewEvent2);
                }
                else
                {
                    var olConsolodateEnrich = _olEnrichmentFactory.CreateEnrichment<EnrichedEvent>(OlEnrichmentType.Adb);
                    var enrichedEvent = await olConsolodateEnrich.ProcessOlMessage(payload);
                    if (enrichedEvent == null)
                    {
                        _logger.LogInformation("Start event, duplicate event, or no context found - eventData: {eventData}", payload);
                        return "";
                    }

                    var purviewEvent = _olToPurviewParsingService.GetPurviewFromOlEvent(enrichedEvent);
                    if (purviewEvent == null)
                    {
                        _logger.LogWarning("No Purview Event found");
                        return "unable to parse purview event";
                    }

                    _logger.LogInformation($"PurviewOut-ParserService: {purviewEvent}");
                    var jObjectPurviewEvent = JsonConvert.DeserializeObject<JObject>(purviewEvent) ?? new JObject();
                    _logger.LogInformation("Calling SendToPurview");
                    await _purviewIngestion.SendToPurview(jObjectPurviewEvent);
                }
                return $"Output message created at {DateTime.Now}";
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ErrorCode: {code}. Error in PurviewOut function {errorMessage}. Input: {eventInput}", ErrorCodes.PurviewOut.GenericError, e.Message, input);                
                return $"Error in PurviewOut function: {e.Message}";
            }
            finally
            {
                if (claimCheckMessage != null)
                {
                    _logger.LogInformation("Deleting claim check: {claimCheckId}", claimCheckMessage.ClaimCheck.Id);
                    await _olClaimCheckService.DeleteClaimCheckAsync(claimCheckMessage.ClaimCheck.Id);
                }
                StopTimer(stopwatch);
            }
        }
        private const string InvalidInputMessageFormat = "Invalid input message format. Unable to parse OlClaimCheckMessage.";
        private OlClaimCheckMessage GetClaimCheckMessage(string input)
        {
            try
            {
                _logger.LogInformation("Parsing input message: {input}", input);
                var claimCheckMessage = JsonConvert.DeserializeObject<OlClaimCheckMessage>(input) ?? throw new Exception(InvalidInputMessageFormat);
                if(claimCheckMessage.ClaimCheck == null)
                {
                    throw new Exception(InvalidInputMessageFormat);
                }
                return claimCheckMessage;
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "{errorMessage}: {eventInput}, error: {error} path: {errorPath}", InvalidInputMessageFormat, input, ex.Message, ex.Path);
                throw;
            }
        }

        private void StopTimer(Stopwatch stopwatch)
        {
            stopwatch.Stop();
            _logger.LogInformation("PurviewOut: Time elapsed: {elapsed}", stopwatch.Elapsed);
        }
    }
}