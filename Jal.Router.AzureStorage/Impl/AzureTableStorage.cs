using System;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureTableStorage : AbstractStorage
    {
        private readonly string _connectionstring;

        private readonly string _sagatablename;

        private readonly string _messagetablename;
        public AzureTableStorage(string connectionstring)
        {
            _connectionstring = connectionstring;

            _sagatablename = "jalsagas";

            _messagetablename = "jalmessages";
        }

        private static CloudTable GetCloudTable(string connectionstring, string tablename)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tablename);

            return table;
        }

        private SagaRecord CreateSaga<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, TData data)
        {
            var table = GetCloudTable(_connectionstring, _sagatablename);

            var record = new SagaRecord($"{context.DateTimeUtc.ToString("yyyyMMdd")}_{Guid.NewGuid()}", saga.DataKeyBuilder(data, context))
            {
                Data = JsonConvert.SerializeObject(data),
                Created = context.DateTimeUtc,
                Name = saga.Name
            };

            table.Execute(TableOperation.Insert(record));

            return record;
        }

        public MessageRecord CreateMessage<TContent>(SagaRecord saga, InboundMessageContext<TContent> context, Route route)
        {
            var table = GetCloudTable(_connectionstring, _messagetablename);

            var record = new MessageRecord(saga.PartitionKey, $"{Guid.NewGuid()}")
            {
                Content = JsonConvert.SerializeObject(context.Content),
                ContentType = route.BodyType.Name,
                Id = context.Id,
                Version = context.Version,
                RetryCount = context.RetryCount,
                Origin = JsonConvert.SerializeObject(context.Origin),
                Headers = JsonConvert.SerializeObject(context.Headers),
                DateTimeUtc = context.DateTimeUtc,
                Name = route.Name,
                Data = saga.Data
            };

            table.Execute(TableOperation.Insert(record));

            return record;
        }

        public SagaRecord GetSaga<TContent>(InboundMessageContext<TContent> context)
        {
            if (context.Headers.ContainsKey("partitionkey") && context.Headers.ContainsKey("rowkey"))
            {
                var table = GetCloudTable(_connectionstring, _sagatablename);

                var partitionkey = context.Headers["partitionkey"];

                var rowkey = context.Headers["rowkey"];

                var result = table.Execute(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey));

                return result.Result as SagaRecord;
            }

            return null;
        }

        public override void Create<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data)
        {
            var record = CreateSaga(saga, context, data);

            UpdateContext(context, record);

            CreateMessage(record, context, route); 
        }

        private static void UpdateContext<TContent>(InboundMessageContext<TContent> context, SagaRecord record)
        {
            if (context.Headers.ContainsKey("partitionkey"))
            {
                context.Headers.Remove("partitionkey");
            }

            context.Headers.Add("partitionkey", record.PartitionKey);

            if (context.Headers.ContainsKey("rowkey"))
            {
                context.Headers.Remove("rowkey");
            }

            context.Headers.Add("rowkey", record.RowKey);
        }

        public override void Update<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data)
        {
            var record = GetSaga(context);

            if (record != null)
            {
                UpdateSaga(record, data);

                CreateMessage(record, context, route);
            }
        }

        private void UpdateSaga<TData>(SagaRecord record, TData data)
        {
            record.Data = JsonConvert.SerializeObject(data);

            var table = GetCloudTable(_connectionstring, _sagatablename);

            table.Execute(TableOperation.Replace(record));
        }

        public override TData Find<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route)
        {
            var record = GetSaga(context);

            if (record!=null)
            {
                return JsonConvert.DeserializeObject<TData>(record.Data);
            }

            return default(TData);
        }
    }
}
