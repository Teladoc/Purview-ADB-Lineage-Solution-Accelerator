using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Function.Domain.Models.Messaging;
using Microsoft.Extensions.Logging;

namespace Function.Domain.Services
{
    public class OlClaimCheckService(IBlobProvider blobProvider, ILogger<OlClaimCheckService> logger) : IOlClaimCheckService
    {
        private const string ContainerName = "olclaimcheck";
        private readonly IBlobProvider _blobProvider = blobProvider ?? throw new ArgumentNullException(nameof(blobProvider));
        private readonly ILogger<OlClaimCheckService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<OlClaimCheck> CreateClaimCheckAsync(string payload)
        {
            var id = $"{DateTime.UtcNow:s}_{Guid.NewGuid()}";
            var blobName = GetBlobName(id);
            await _blobProvider.UploadAsync(ContainerName, blobName, payload);
            _logger.LogInformation("Created claimcheck {claimCheck}", blobName);
            return new OlClaimCheck(id, await GetChecksumAsync(payload));
        }        

        public async Task<string> GetClaimCheckPayloadAsync(string id)
        {
            if (!await _blobProvider.BlobExistsAsync(ContainerName, GetBlobName(id)))
            {
                return string.Empty;
            }
            return await _blobProvider.DownloadBlobAsync(ContainerName, GetBlobName(id));
        }        

        public async Task DeleteClaimCheckAsync(string id)
        {
            await _blobProvider.DeleteAsync(ContainerName, GetBlobName(id));
        }

        private static string GetBlobName(string id)
        {
            return $"{id}.json";
        }

        private static async Task<string> GetChecksumAsync(string payload)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(payload);
            MemoryStream stream = new(byteArray);
            var hashBytes = await SHA256.HashDataAsync(stream);
            return Convert.ToBase64String(hashBytes);
        }
    }
}