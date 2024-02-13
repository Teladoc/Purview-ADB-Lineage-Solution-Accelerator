// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using Function.Domain.Models.OL;

namespace Function.Domain.Middleware
{
    public class ScopedLoggingMiddleware : IFunctionsWorkerMiddleware
    {
        const string CORRELATION_ID_HEADER_NAME = "CorrelationID";

        private const string ContextDataKey = "run";

        public string CorrelationId = "";

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            object? bindingData = null;
            if (!(context?.BindingContext?.BindingData.TryGetValue(ContextDataKey, out bindingData)).GetValueOrDefault(false))
            {
                await next(context!);
            }
            if (bindingData == null)
            {
                await next(context!);
            }
            ILogger logger = context!.GetLogger<ScopedLoggingMiddleware>();

            try
            {
                logger.LogInformation("ScopedLoggingMiddleware: BindingData is {bindingData}", bindingData);

                object runobj = bindingData ?? new();
                string runjson = runobj?.ToString() ?? "";
                var runFacet = JsonConvert.DeserializeObject<Run>(runjson) ?? new Run();
                CorrelationId = runFacet.RunId;

                if (string.IsNullOrEmpty(CorrelationId))
                {
                    CorrelationId = Guid.NewGuid().ToString();
                    logger.LogWarning($"No CorrelationId found in request. Generated new CorrelationId: {CorrelationId}");
                }
            }
            catch (JsonReaderException ex)
            {
                logger.LogError(ex, "ScopedLoggingMiddleware: Error deserializing binding data to Run. BindingData: {bindingData}. JSON Path: {path}", bindingData, ex.Path);
                await next(context!);
                return;
            }
            

            try
            {
                var loggerState = new Dictionary<string, object>
                {
                    { CORRELATION_ID_HEADER_NAME, CorrelationId }
                };


                using (logger.BeginScope(loggerState))
                {
                    await next(context!);
                }
            }
            //To make sure that we don't loose the scope in case of an unexpected error
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected exception occurred!");
                return;
            }
        }
    }
}