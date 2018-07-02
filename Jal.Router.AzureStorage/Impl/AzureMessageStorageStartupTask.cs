using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureMessageStorageStartupTask : IStartupTask
    {
        private readonly string _connectionstring;

        private readonly string _container;

        private readonly ILogger _logger;

        public AzureMessageStorageStartupTask(ILogger logger,string connectionstring, string container)
        {
            _connectionstring = connectionstring;

            _container = container;

            _logger = logger;
        }

        private static CloudBlobContainer GetContainer(string connectionstring, string containername)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containername);

            return container;
        }

        public void Run()
        {
            var container = GetContainer(_connectionstring, _container);

            var result = container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            _logger.Log($"Created {_container} container");
        }
    }
}