using System;

namespace Jal.Router.AzureStorage.Model
{
    public class AzureStorageParameter
    {
        public string TableStorageConnectionString { get; set; }

        public string BlobConnectionString { get; set; }

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
        }

        public AzureStorageParameter(string connectionstring)
        {
            SagaTableName = "sagas";

            MessageTableName = "messages";

            TableSufix = DateTime.UtcNow.ToString("yyyyMM");

            ContainerName = "messages";

            TableStorageConnectionString = connectionstring;

            BlobConnectionString = connectionstring;
        }

        public AzureStorageParameter(string tablestorageconnectionstring, string blobconnectionstring)
        {
            SagaTableName = "sagas";

            MessageTableName = "messages";

            TableSufix = DateTime.UtcNow.ToString("yyyyMM");

            ContainerName = "messages";

            TableStorageConnectionString = tablestorageconnectionstring;

            BlobConnectionString = blobconnectionstring;
        }
    }
}