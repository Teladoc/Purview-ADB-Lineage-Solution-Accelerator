using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using System.Linq;

namespace Function.Domain.Helpers
{
    public class HttpHelper : IHttpHelper
    {
        public async Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }
        public HttpResponseData CreateServerErrorHttpResponse(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);

            return response;
        }

        public HttpResponseData CreateUnauthorizedHttpResponse(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.Forbidden);
            return response;
        }

        public bool ValidateRequestHeaders(HttpRequestData req, string sourceHeaderExpectedValue)
        {            
            if (req.Headers.TryGetValues("x-teladoc-udf-ol-source", out var values))
            {
                return values.Contains(sourceHeaderExpectedValue, System.StringComparer.OrdinalIgnoreCase);
            }
            return false;
        }

        public async Task<HttpResponseData> CreateBadRequestHttpResponse(HttpRequestData req, string message)
        {
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(System.Text.Json.JsonSerializer.Serialize(new { message }), System.Text.Encoding.UTF8);
            return response;
        }

    }
}