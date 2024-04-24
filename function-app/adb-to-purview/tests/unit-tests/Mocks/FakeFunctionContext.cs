using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    public class FakeFunctionContext : FunctionContext
    {
        public override string InvocationId => Guid.NewGuid().ToString();

        public override string FunctionId => Guid.NewGuid().ToString();

        public override TraceContext TraceContext => throw new NotImplementedException();

        public override BindingContext BindingContext => throw new NotImplementedException();

        public override RetryContext RetryContext => throw new NotImplementedException();

        public override IServiceProvider InstanceServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override FunctionDefinition FunctionDefinition => throw new NotImplementedException();

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IInvocationFeatures Features => throw new NotImplementedException();
    }

}
