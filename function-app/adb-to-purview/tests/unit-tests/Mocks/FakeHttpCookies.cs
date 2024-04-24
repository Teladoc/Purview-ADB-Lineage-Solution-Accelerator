using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    public class FakeHttpCookies : HttpCookies
    {
        public override void Append(string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void Append(IHttpCookie cookie)
        {
            throw new NotImplementedException();
        }

        public override IHttpCookie CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
