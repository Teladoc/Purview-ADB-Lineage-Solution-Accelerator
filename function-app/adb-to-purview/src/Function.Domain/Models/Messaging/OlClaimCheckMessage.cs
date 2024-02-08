using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Function.Domain.Models.Messaging
{
    public class OlClaimCheckMessage(OlClaimCheck claimCheck)
    {
        public OlClaimCheck ClaimCheck { get; init; } = claimCheck;
    }
}