using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureTableStorage : AbstractStorage
    {
        private readonly string _connectionstring;

        private readonly string _sagastoragename;

        private readonly string _messagestorgename;

        private readonly string _currenttablenamesufix;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public AzureTableStorage(string connectionstring, IComponentFactory factory, IConfiguration configuration, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            _connectionstring = connectionstring;

            _factory = factory;

            _configuration = configuration;

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

        public override SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{_currenttablenamesufix}");

            if (!string.IsNullOrWhiteSpace(sagastoragename))
            {
                table = GetCloudTable(_connectionstring, $"{sagastoragename}");
            }

            var diff = (end - start).TotalDays;

            var currentdate = new DateTime(start.Year, start.Month, start.Day);

            var list = new List<SagaEntity>();

            for (var i = 0; i <= diff; i++)
            {
                var partitionkey = $"{currentdate.ToString("yyyyMMdd")}_{saganame}";

                var sagas = GetSagas(table, partitionkey, start, end);

                list.AddRange(sagas);

                currentdate =currentdate.AddDays(1);
            }

            return list.ToArray();
        }

        private SagaEntity[] GetSagas(CloudTable table, string partitionkey, DateTime start, DateTime end)
        {
            var records = table.CreateQuery<SagaRecord>().Where(x => x.PartitionKey == partitionkey && x.Created >= start && x.Created < end);

            return records.Select(x => new SagaEntity()
            {
                Data = x.Data,
                DataType = x.DataType,
                Name = x.Name,
                Created = x.Created,
                Updated = x.Updated,
                Ended = x.Ended,
                Timeout = x.Timeout,
                Status = x.Status,
                Duration = x.Duration,
                Key = x.RowKey
            }).ToArray();
        }

        public override MessageEntity[] GetMessagesBySaga(SagaEntity entity, string messagestoragename = "")
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var table = GetCloudTable(_connectionstring, $"{_messagestorgename}{_currenttablenamesufix}");

            if (!string.IsNullOrWhiteSpace(messagestoragename))
            {
                table = GetCloudTable(_connectionstring, $"{messagestoragename}");
            }

            var records = table.CreateQuery<MessageRecord>().Where(x => x.PartitionKey == entity.Key);

            return records.Select(x => new MessageEntity()
            {
                Content = x.Content,
                ContentType = x.ContentType,
                Id = x.Id,
                Version = x.Version,
                RetryCount = x.RetryCount,
                LastRetry = x.LastRetry,
                Origin = serializer.Deserialize<Origin>(x.Origin),
                Saga = serializer.Deserialize<SagaInfo>(x.Saga),
                Headers = serializer.Deserialize<Dictionary<string, string>>(x.Headers),
                DateTimeUtc = x.DateTimeUtc,
                Data = x.Data,
                Name = x.Name
            }).ToArray();
        }

        public override MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            var table = GetCloudTable(_connectionstring, $"{_messagestorgename}{_currenttablenamesufix}");

            if (!string.IsNullOrWhiteSpace(messagestoragename))
            {
                table = GetCloudTable(_connectionstring, $"{messagestoragename}");
            }

            var diff = (int)(end - start).TotalDays;

            var currentdate = new DateTime(start.Year, start.Month, start.Day);

            var list = new List<MessageEntity>();

            for (var i = 0; i <= diff; i++)
            {
                var partitionkey = $"{currentdate.ToString("yyyyMMdd")}_{routename}";

                var messages = GetMessages(table, partitionkey, start, end);

                list.AddRange(messages);

                currentdate = currentdate.AddDays(1);
            }

            return list.ToArray();
        }

        private MessageEntity[] GetMessages(CloudTable table, string partitionkey, DateTime start, DateTime end)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var records = table.CreateQuery<MessageRecord>().Where(x => x.PartitionKey == partitionkey && x.DateTimeUtc >= start && x.DateTimeUtc < end);

            return records.Select(x => new MessageEntity()
            {
                Content = x.Content,
                ContentType = x.ContentType,
                Id = x.Id,
                Version = x.Version,
                RetryCount = x.RetryCount,
                LastRetry = x.LastRetry,
                Origin = !string.IsNullOrWhiteSpace(x.Origin) ? serializer.Deserialize<Origin>(x.Origin) : null,
                Saga = !string.IsNullOrWhiteSpace(x.Saga) ? serializer.Deserialize<SagaInfo>(x.Saga) : null,
                Headers = !string.IsNullOrWhiteSpace(x.Headers) ? serializer.Deserialize<Dictionary<string, string>>(x.Headers) : null,
                Tracks = !string.IsNullOrWhiteSpace(x.Tracks) ? serializer.Deserialize<List<Track>>(x.Tracks) : null,
                DateTimeUtc = x.DateTimeUtc,
                Data = x.Data,
                Name = x.Name,
            }).ToArray();
        }

        public override void CreateMessage(MessageContext context, string id, SagaEntity sagaentity, MessageEntity messageentity)
        {
            try
            {
                var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

                var tablenamesufix = GetTableNameSufix(id);

                var table = GetCloudTable(_connectionstring, $"{_messagestorgename}{tablenamesufix}");

                var record = new MessageRecord(sagaentity.Key, $"{messageentity.Name}_{Guid.NewGuid()}")
                {
                    Content = messageentity.Content,
                    ContentType = messageentity.ContentType,
                    Id = messageentity.Id,
                    Version = messageentity.Version,
                    RetryCount = messageentity.RetryCount,
                    LastRetry = messageentity.LastRetry,
                    Origin = serializer.Serialize(messageentity.Origin),
                    Saga = serializer.Serialize(messageentity.Saga),
                    Headers = serializer.Serialize(messageentity.Headers),
                    Tracks = serializer.Serialize(messageentity.Tracks),
                    DateTimeUtc = messageentity.DateTimeUtc,
                    Data = messageentity.Data,
                    Name = messageentity.Name,
                };

                var size = Encoding.UTF8.GetByteCount(record.Content);

                if (size >= 64000)
                {
                    record.Content = LimitByteLength(record.Content, 63000) + "...";
                }

                table.Execute(TableOperation.Insert(record));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation saga {context.Saga.Name} with content {context.ContentType.FullName}", ex);
            }
        }

        private static string LimitByteLength(string input, int maxLength)
        {
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (Encoding.UTF8.GetByteCount(input.Substring(0, i + 1)) <= maxLength)
                {
                    return input.Substring(0, i + 1);
                }
            }

            return string.Empty;
        }

        public override SagaEntity GetSaga(string id)
        {
            try
            {
                var entity = new SagaEntity() { Key = id };

                var tablenamesufix = GetTableNameSufix(id);

                var partitionkey = GetPartitionKey(id);

                var rowkey = GetRowKey(id);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{tablenamesufix}");

                    var result = table.Execute(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey));

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        entity.Data = record.Data;

                        entity.DataType = record.DataType;

                        entity.Name = record.Name;

                        entity.Created = record.Created;

                        entity.Updated = record.Updated;

                        entity.Ended = record.Ended;

                        entity.Timeout = record.Timeout;

                        entity.Status = record.Status;

                        entity.Duration = record.Duration;

                        entity.Key = record.RowKey;

                        return entity;
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga key {id}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga key format {id}");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record lookup saga key {id}", ex);
            }
        }

        public override string CreateSaga(MessageContext context, SagaEntity entity)
        {
            try
            {
                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{context.Saga.Name}";

                var id = $"{partition}@{entity.Key}@{_currenttablenamesufix}";
                
                var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{_currenttablenamesufix}");

                var record = new SagaRecord(partition, entity.Key)
                {
                    Data = entity.Data,
                    Created = entity.Created,
                    Updated = entity.Updated,
                    Name = entity.Name,
                    DataType = entity.DataType,
                    Timeout = entity.Timeout,
                    Status = entity.Status
                };

                table.Execute(TableOperation.Insert(record));

                return id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record creation saga {context.Saga.Name}", ex);
            }
        }

        public override void CreateMessage(MessageContext context, MessageEntity entity)
        {
            try
            {
                var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{context.Route.Name}";

                var table = GetCloudTable(_connectionstring, $"{_messagestorgename}{_currenttablenamesufix}");

                var record = new MessageRecord(partition, $"{Guid.NewGuid()}")
                {
                    Content = entity.Content,
                    ContentType = entity.ContentType,
                    Id = entity.Id,
                    Version = entity.Version,
                    RetryCount = entity.RetryCount,
                    LastRetry = entity.LastRetry,
                    Origin = serializer.Serialize(entity.Origin),
                    Headers = serializer.Serialize(entity.Headers),
                    DateTimeUtc = entity.DateTimeUtc,
                    Name = entity.Name,
                    Tracks = serializer.Serialize(entity.Tracks),
                };

                table.Execute(TableOperation.Insert(record));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation with content {context.ContentType.FullName}", ex);
            }
        }

        public override void UpdateSaga(MessageContext context, string id, SagaEntity entity)
        {
            try
            {
                var tablenamesufix = GetTableNameSufix(id);

                var partitionkey = GetPartitionKey(id);

                var rowkey = GetRowKey(id);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_connectionstring, $"{_sagastoragename}{tablenamesufix}");

                    var result = table.Execute(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey));

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        record.Data = entity.Data;

                        record.Updated = entity.Updated;

                        if (!string.IsNullOrWhiteSpace(entity.Status))
                        {
                            record.Status = entity.Status;
                        }

                        record.Duration = entity.Duration;

                        if (entity.Ended != null)
                        {
                            record.Ended = entity.Ended;
                        }

                        record.ETag = "*";

                        table.Execute(TableOperation.Replace(record));
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga key {entity.Key}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga key format {entity.Key}");
                }
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                {
                    throw new ApplicationException($"Error during the saga update (Optimistic concurrency violation – entity has changed since it was retrieved) {entity.Name}", ex);
                }
                else
                {
                    throw new ApplicationException($"Error during the saga update ({ex.RequestInformation.HttpStatusCode}) {entity.Name}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga update {entity.Name}", ex);
            }
        }

        public static string GetPartitionKey(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var parts = id.Split('@');

                if (parts.Length >= 2)
                {
                    return parts[0];
                }
            }

            return string.Empty;
        }

        public static string GetRowKey(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var parts = id.Split('@');

                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }

            return string.Empty;
        }

        public static string GetTableNameSufix(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var parts = id.Split('@');

                if (parts.Length == 3)
                {
                    return parts[2];
                }
            }

            return string.Empty;
        }
    }
}
