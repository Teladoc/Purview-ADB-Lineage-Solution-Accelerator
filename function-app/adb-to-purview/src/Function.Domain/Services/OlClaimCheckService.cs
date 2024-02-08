using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Function.Domain.Models.Messaging;

namespace Function.Domain.Services
{
    public class OlClaimCheckService(IBlobProvider blobProvider) : IOlClaimCheckService
    {
        private const string ContainerName = "olclaimcheck";
        private readonly IBlobProvider _blobProvider = blobProvider ?? throw new ArgumentNullException(nameof(blobProvider));

        public async Task<OlClaimCheck> CreateClaimCheckAsync(string payload)
        {
            var id = $"{DateTime.UtcNow:s}_{Guid.NewGuid()}";
            await _blobProvider.UploadAsync(ContainerName, GetBlobName(id), payload);
            return new OlClaimCheck(id, await GetChecksumAsync(payload));
        }        

        public Task<string> GetClaimCheckPayloadAsync(string id)
        {
            return _blobProvider.DownloadBlobAsync(ContainerName, GetBlobName(id));
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