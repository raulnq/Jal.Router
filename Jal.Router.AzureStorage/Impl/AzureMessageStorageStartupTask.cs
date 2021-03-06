﻿using System.Threading.Tasks;
using Jal.Router.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Jal.Router.AzureStorage
{
    public class AzureMessageStorageStartupTask : IStartupTask
    {
        private readonly ILogger _logger;

        private readonly AzureStorageParameter _parameter;

        public AzureMessageStorageStartupTask(ILogger logger, IParameterProvider provider)
        {
            _parameter = provider.Get<AzureStorageParameter>();

            _logger = logger;
        }

        private static CloudBlobContainer GetContainer(string connectionstring, string containername)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containername);

            return container;
        }

        public async Task Run()
        {
            if(!string.IsNullOrEmpty(_parameter.BlobStorageConnectionString))
            {
                var container = GetContainer(_parameter.BlobStorageConnectionString, _parameter.ContainerName);

                var result = await container.CreateIfNotExistsAsync().ConfigureAwait(false);

                if (result)
                {
                    _logger.Log($"Created {_parameter.ContainerName} container");
                }
                else
                {
                    _logger.Log($"Container {_parameter.ContainerName} already exists");
                }
            }
            else
            {
                _logger.Log($"Skipped creation of container for messages");
            }
            
        }
    }
}