using System;
using System.Net;
using Jal.Router.AzureStorage.Extensions;
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

        private readonly string _sagastoragename;

        private readonly string _messagestorgename;

        private readonly string _currenttablenamesufix;

        public AzureTableStorage(string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            _connectionstring = connectionstring;

            _sagastoragename = sagastoragename;

            _messagestorgename = messagestorgename;

            _currenttablenamesufix = tablenamesufix;
        }

        private static CloudTable GetCloudTable(string connectionstring, string tablename)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tablename);

            return table;
        }

        private void CreateSaga<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, TData data)
        {
            try
            {
                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{saga.Name}";

                var row = Guid.NewGuid().ToString();

                context.Saga.SetId(partition, row, _currenttablenamesufix);

                var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{_currenttablenamesufix}");

                var record = new SagaRecord(partition, row)
                {
                    Data = JsonConvert.SerializeObject(data),
                    Created = context.DateTimeUtc,
                    Name = saga.Name,
                    DataType = saga.DataType.FullName
                };

                table.Execute(TableOperation.Insert(record));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record creation saga {saga.Name}", ex);
            }
        }

        private void CreateMessage<TContent>(SagaRecord saga, InboundMessageContext<TContent> context, Route route, string tablenamesufix)
        {
            try
            {
                var table = GetCloudTable(_connectionstring, $"{_messagestorgename}{tablenamesufix}");

                var record = new MessageRecord(saga.RowKey, $"{route.BodyType.Name}_{Guid.NewGuid()}")
                {
                    Content = JsonConvert.SerializeObject(context.Content),
                    ContentType = route.BodyType.FullName,
                    Id = context.Id,
                    Version = context.Version,
                    RetryCount = context.RetryCount,
                    LastRetry = context.LastRetry,
                    Origin = JsonConvert.SerializeObject(context.Origin),
                    Saga = JsonConvert.SerializeObject(context.Saga),
                    Headers = JsonConvert.SerializeObject(context.Headers),
                    DateTimeUtc = context.DateTimeUtc,
                    Name = route.Name,
                    Data = saga.Data
                };

                table.Execute(TableOperation.Insert(record));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation saga {saga.Name} and route {route.Name}", ex);
            }
        }

        public SagaRecord GetSaga<TContent>(InboundMessageContext<TContent> context, Saga saga, string tablenamesufix)
        {
            try
            {
                var partitionkey = context.Saga.GetPartitionKey();

                var rowkey = context.Saga.GetRowKey();

                if (!string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {

                        var table = GetCloudTable(_connectionstring,$"{_sagastoragename}{tablenamesufix}");

                        var result = table.Execute(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey));

                        return result.Result as SagaRecord;
                    
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record lookup saga {saga.Name}", ex);
            }

            return null;
        }

        public override void Create<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data)
        {
            CreateSaga(saga, context, data);
        }


        public override void Update<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data)
        {
            var tablenamesufix = context.Saga.GetTableNameSufix();

            var record = GetSaga(context, saga, tablenamesufix);

            if (record != null)
            {
                UpdateSaga(record, data, tablenamesufix);

                CreateMessage(record, context, route, tablenamesufix);
            }
        }

        private void UpdateSaga<TData>(SagaRecord record, TData data, string tablenamesufix)
        {
            try
            {
                record.Data = JsonConvert.SerializeObject(data);

                var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{tablenamesufix}");

                table.Execute(TableOperation.Replace(record));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                {
                    throw new ApplicationException($"Error during the saga update (Optimistic concurrency violation – entity has changed since it was retrieved) {record.Name}", ex);
                }
                else
                {
                    throw new ApplicationException($"Error during the saga update ({ex.RequestInformation.HttpStatusCode}) {record.Name}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga update {record.Name}", ex);
            }
        }

        public override TData Find<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route)
        {
            var tablenamesufix = context.Saga.GetTableNameSufix();

            var record = GetSaga(context, saga, tablenamesufix);

            if (record!=null)
            {
                return JsonConvert.DeserializeObject<TData>(record.Data);
            }

            return null;
        }
    }
}
