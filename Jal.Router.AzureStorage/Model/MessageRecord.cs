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
            NumberOfSagaArrays = 0;
            NumberOfContentArrays = 0;
            SizeOfSagaArraysOnKilobytes = 0;
            SizeOfContentArraysOnKilobytes = 0;
        }

        public MessageRecord()
        {
            NumberOfSagaArrays = 0;
            NumberOfContentArrays = 0;
            SizeOfSagaArraysOnKilobytes = 0;
            SizeOfContentArraysOnKilobytes = 0;
        }

        public byte[] Saga0 { get; set; }
        public byte[] Saga1 { get; set; }
        public byte[] Saga2 { get; set; }
        public byte[] Saga3 { get; set; }
        public byte[] Saga4 { get; set; }
        public int NumberOfSagaArrays { get; set; }
        public int SizeOfSagaArraysOnKilobytes { get; set; }
        public int SizeOfSaga { get; set; }
        public int SizeOfContent { get; set; }
        public byte[] Content0 { get; set; }
        public byte[] Content1 { get; set; }
        public byte[] Content2 { get; set; }
        public byte[] Content3 { get; set; }
        public byte[] Content4 { get; set; }
        public int NumberOfContentArrays { get; set; }
        public int SizeOfContentArraysOnKilobytes { get; set; }
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ChannelEntity { get; set; }
        public string Headers { get; set; }
        public string Version { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
        public string RouteEntity { get; set; }
        public string EndpointEntity { get; set; }
        public string OriginEntity { get; set; }
        public string SagaEntity { get; set; }
        public string ContentContextEntity { get; set; }
        public string SagaContextEntity { get; set; }
        public string TrackingContextEntity { get; set; }
        public string IdentityContextEntity { get; set; }
    }
}