using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Function.Domain.Models.Messaging
{
    public class OlClaimCheck(string id, string checksum)
    {
        public string Id { get; init; } = id;
        public string Checksum { get; init; } = checksum;
    }
}