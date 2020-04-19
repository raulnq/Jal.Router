using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Jal.Router.AzureStorage
{
    public class AzureMessageStorage : IMessageStorage
    {
        private readonly AzureStorageParameter _parameter;

        public AzureMessageStorage(string connectionstring, string path, IParameterProvider provider)
        {
            _parameter = provider.Get<AzureStorageParameter>();
        }

        private static CloudBlobContainer GetContainer(string connectionstring, string containername)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containername);

            return container;
        }


        public async Task<string> Read(string id)
        {
            try
            {
                var container = GetContainer(_parameter.BlobStorageConnectionString, _parameter.ContainerName);

                var blob = container.GetBlockBlobReference(id);

                using (var memorystream = new System.IO.MemoryStream())
                {
                    await blob.DownloadToStreamAsync(memorystream).ConfigureAwait(false);

                    string result = Encoding.UTF8.GetString(memorystream.ToArray());

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to read the message with contentid {id}", ex); ;
            }
        }

        public async Task Write(string id, string content)
        {
            try
            {
                var container = GetContainer(_parameter.BlobStorageConnectionString, _parameter.ContainerName);

                var blob = container.GetBlockBlobReference(id);

                using (var memorystream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    await blob.UploadFromStreamAsync(memorystream).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to write the message with contentid {id}", ex); ;
            }
        }
    }
}
