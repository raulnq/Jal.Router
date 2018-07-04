using System;
using System.Text;
using Jal.Router.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureMessageStorage : IMessageStorage
    {
        private readonly string _connectionstring;

        private readonly string _path;

        public AzureMessageStorage(string connectionstring, string path)
        {
            _connectionstring = connectionstring;
            _path = path;
        }

        private static CloudBlobContainer GetContainer(string connectionstring, string containername)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containername);

            return container;
        }


        public string Read(string id)
        {
            try
            {
                var container = GetContainer(_connectionstring, _path);

                var blob = container.GetBlockBlobReference(id);

                using (var memorystream = new System.IO.MemoryStream())
                {
                    blob.DownloadToStreamAsync(memorystream).GetAwaiter().GetResult();

                    string result = Encoding.UTF8.GetString(memorystream.ToArray());

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to read the message with dataid {id}", ex); ;
            }
        }

        public void Write(string id, string content)
        {
            try
            {
                var container = GetContainer(_connectionstring, _path);

                var blob = container.GetBlockBlobReference(id);

                using (var memorystream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    blob.UploadFromStreamAsync(memorystream).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to write the message with dataid {id}", ex); ;
            }
        }
    }
}
