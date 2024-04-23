using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace Function.Domain.Helpers
{
    public interface IHttpHelper
    {
         public Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data);
         public HttpResponseData CreateServerErrorHttpResponse(HttpRequestData req);
         public HttpResponseData CreateUnauthorizedHttpResponse(HttpRequestData req);
         public bool ValidateRequestHeaders(HttpRequestData req, string sourceHeaderExpectedValue);
    }
}