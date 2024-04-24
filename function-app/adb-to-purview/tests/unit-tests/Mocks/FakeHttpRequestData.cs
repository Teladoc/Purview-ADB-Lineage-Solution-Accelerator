using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http;

namespace UnitTests.Mocks
{
    public class FakeHttpRequestData(FunctionContext functionContext, Uri url, Stream? body = null) : HttpRequestData(functionContext)
    {
        public FakeHttpRequestData() : this(new FakeFunctionContext(), new Uri("http://fakeurl.test"))
        {

        }

        public override Stream Body { get; } = body ?? new MemoryStream();

        public override HttpHeadersCollection Headers { get; } = [];

        public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = [];

        public override Uri Url { get; } = url;

        public override IEnumerable<ClaimsIdentity> Identities { get; } = [];

        public override string Method { get; } = HttpMethod.Get.Method;

        public override HttpResponseData CreateResponse()
        {
            return new FakeHttpResponseData(FunctionContext);
        }
    }  
}
