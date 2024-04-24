using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace UnitTests.Mocks
{
    public class FakeHttpRequestData : HttpRequestData
    {
        public FakeHttpRequestData() : this(new FakeFunctionContext(), new Uri("http://fakeurl.test"))
        {

        }

        public FakeHttpRequestData(FunctionContext functionContext, Uri url, Stream body = null) : base(functionContext)
        {
            Url = url;
            Body = body ?? new MemoryStream();
        }

        public override Stream Body { get; } = new MemoryStream();

        public override HttpHeadersCollection Headers { get; } = [];

        public override IReadOnlyCollection<IHttpCookie> Cookies { get; }

        public override Uri Url { get; }

        public override IEnumerable<ClaimsIdentity> Identities { get; }

        public override string Method { get; }

        public override HttpResponseData CreateResponse()
        {
            return new FakeHttpResponseData(FunctionContext);
        }
    }

}
