using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Model
{
    public class MessageRecord : TableEntity
    {
        public MessageRecord(string partitionkey, string rowkey)
        {
            PartitionKey = partitionkey;
            RowKey = rowkey;
            NumberOfDataArrays = 0;
            NumberOfContentArrays = 0;
            SizeOfDataArraysOnKilobytes = 0;
            SizeOfContentArraysOnKilobytes = 0;
        }

        public MessageRecord()
        {
            NumberOfDataArrays = 0;
            NumberOfContentArrays = 0;
            SizeOfDataArraysOnKilobytes = 0;
            SizeOfContentArraysOnKilobytes = 0;
        }

        public string Tracks { get; set; }
        public string Data { get; set; }
        public byte[] Data0 { get; set; }
        public byte[] Data1 { get; set; }
        public byte[] Data2 { get; set; }
        public byte[] Data3 { get; set; }
        public byte[] Data4 { get; set; }
        public int NumberOfDataArrays { get; set; }
        public int SizeOfDataArraysOnKilobytes { get; set; }
        public int SizeOfData { get; set; }
        public int SizeOfContent { get; set; }
        public string ContentId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public byte[] Content0 { get; set; }
        public byte[] Content1 { get; set; }
        public byte[] Content2 { get; set; }
        public byte[] Content3 { get; set; }
        public byte[] Content4 { get; set; }
        public int NumberOfContentArrays { get; set; }
        public int SizeOfContentArraysOnKilobytes { get; set; }
        public string Identity { get; set; }

        public string Version { get; set; }

        public int RetryCount { get; set; }

        public bool LastRetry { get; set; }
        public string SagaId { get; set; }
        public string Origin { get; set; }
        public string Saga { get; set; }
        public string Headers { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Id { get; set; }
    }
}