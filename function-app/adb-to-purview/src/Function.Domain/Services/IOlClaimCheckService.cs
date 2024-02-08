using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Function.Domain.Models.Messaging;

namespace Function.Domain.Services
{
    public interface IOlClaimCheckService
    {
        Task<OlClaimCheck> CreateClaimCheckAsync(string payload);
        Task<string> GetClaimCheckPayloadAsync(string id);
        Task DeleteClaimCheckAsync(string id);
    }
}