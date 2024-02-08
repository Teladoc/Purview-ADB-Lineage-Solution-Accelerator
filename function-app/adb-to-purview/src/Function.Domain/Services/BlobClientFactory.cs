using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Azure.Storage.Blobs.Models;
using Azure.Core;

namespace Function.Domain.Services
{
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobClientFactory(IConfiguration config, ILogger<BlobClientFactory> logger)
        {
            var tenantId = config["TenantId"];
            var msiClientId = config["UserMsiId"];
            var accountName = config["OlMessageStoreAccountName"];

            var credentialConfig = new DefaultAzureCredentialOptions()
            {
                TenantId = tenantId,
                ExcludeAzureCliCredential = false
            };
            if (Convert.ToBoolean(config["UseUserMsi"] ?? "false"))
            {
                credentialConfig.ManagedIdentityClientId = msiClientId;
            }
            BlobClientOptions blobOptions = new()
            {
                Retry = {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential,
                    MaxDelay = TimeSpan.FromSeconds(10),
                    NetworkTimeout = TimeSpan.FromSeconds(100)
                },
            };
            _blobServiceClient = new(
                new Uri($"https://{accountName}.blob.core.windows.net"),
                new DefaultAzureCredential(credentialConfig),
                blobOptions);
            logger.LogInformation("BlobClientFactory created for {accountName}", accountName);
        }

        public async Task<BlobClient> GetBlobClientAsync(string containerName, string name)
        {
            var containerClient = await this.GetBlobContainerClientAsync(containerName);
            return containerClient.GetBlobClient(name);
        }

        public async Task<BlobContainerClient> GetBlobContainerClientAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
            return containerClient;
        }
    }
}