using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Function.Domain.Models.Messaging
{
    [method: JsonConstructor]
    public class OlClaimCheckMessage(OlClaimCheck claimCheck)
    {
        public OlClaimCheck ClaimCheck { get; init; } = claimCheck;
    }
}