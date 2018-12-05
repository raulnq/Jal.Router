using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Model
{
    public class SagaRecord : TableEntity
    {
        public SagaRecord(string partitionkey, string rowkey)
        {
            PartitionKey = partitionkey;
            RowKey = rowkey;
            NumberOfDataArrays = 0;
            SizeOfDataArraysOnKilobytes = 0;
        }

        public SagaRecord()
        {
            NumberOfDataArrays = 0;
            SizeOfDataArraysOnKilobytes = 0;
        }

        public string Data { get; set; }
        public string Id { get; set; }
        public byte[] Data0 { get; set; }
        public byte[] Data1 { get; set; }
        public byte[] Data2 { get; set; }
        public byte[] Data3 { get; set; }
        public byte[] Data4 { get; set; }
        public int NumberOfDataArrays { get; set; }
        public int SizeOfDataArraysOnKilobytes { get; set; }
        public string DataType { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public DateTime? Ended { get; set; }

        public int? Timeout { get; set; }

        public string Status { get; set; }

        public double Duration { get; set; }
    }
}