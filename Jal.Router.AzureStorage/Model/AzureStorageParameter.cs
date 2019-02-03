using System;
using System.Text;

namespace Jal.Router.AzureStorage.Model
{
    public class AzureStorageParameter
    {
        public string TableStorageConnectionString { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public Encoding TableStorageStringEncoding { get; set; }

        public int TableStorageMaxColumnSizeOnKilobytes { get; set; }

        public string SagaTableName { get; set; }

        public string MessageTableName { get; set; }

        public string TableSufix { get; set; }

        public string ContainerName { get; set; }

        public AzureStorageParameter()
        {
            SagaTableName = "sagas";

            MessageTableName = "messages";

            TableSufix = DateTime.UtcNow.ToString("yyyyMM");

            ContainerName = "messages";

            TableStorageStringEncoding = Encoding.Unicode;

            TableStorageMaxColumnSizeOnKilobytes = 64;
        }

        public AzureStorageParameter(string connectionstring)
        {
            SagaTableName = "sagas";

            MessageTableName = "messages";

            TableSufix = DateTime.UtcNow.ToString("yyyyMM");

            ContainerName = "messages";

            TableStorageConnectionString = connectionstring;

            BlobStorageConnectionString = connectionstring;

            TableStorageStringEncoding = Encoding.Unicode;

            TableStorageMaxColumnSizeOnKilobytes = 64;
        }

        public AzureStorageParameter(string tablestorageconnectionstring, string blobconnectionstring)
        {
            SagaTableName = "sagas";

            MessageTableName = "messages";

            TableSufix = DateTime.UtcNow.ToString("yyyyMM");

            ContainerName = "messages";

            TableStorageConnectionString = tablestorageconnectionstring;

            BlobStorageConnectionString = blobconnectionstring;

            TableStorageStringEncoding = Encoding.Unicode;

            TableStorageMaxColumnSizeOnKilobytes = 64;
        }
    }
}