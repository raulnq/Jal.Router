using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Impl.Inbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureEntityStorage : AbstractEntityStorage
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly AzureStorageParameter _parameter;

        private readonly int _kilobyte = 1024;

        private readonly Action<MessageRecord, byte[]>[] MessageContentWriter;

        private readonly Action<MessageRecord, byte[]>[] MessageDataWriter;

        private readonly Action<SagaRecord, byte[]>[] SagaDataWriter;

        private readonly Func<SagaRecord, byte[]>[] SagaDataReader;

        private readonly Func<MessageRecord, byte[]>[] MessageContentReader;

        private readonly Func<MessageRecord, byte[]>[] MessageDataReader;

        public AzureEntityStorage(IComponentFactory factory, IConfiguration configuration, IParameterProvider provider)
        {
            _factory = factory;

            _configuration = configuration;

            _parameter = provider.Get<AzureStorageParameter>();

            MessageContentWriter = new Action<MessageRecord, byte[]>[] { (x, y) => x.Content0 = y, (x, y) => x.Content1 = y , (x, y) => x.Content2 = y , (x, y) => x.Content3 = y , (x, y) => x.Content4 = y };

            MessageDataWriter = new Action<MessageRecord, byte[]>[] { (x, y) => x.Data0 = y, (x, y) => x.Data1 = y, (x, y) => x.Data2 = y, (x, y) => x.Data3 = y, (x, y) => x.Data4 = y };

            MessageContentReader = new Func<MessageRecord, byte[]>[] { (x) => x.Content0, (x) => x.Content1, (x) => x.Content2, (x) => x.Content3, (x) => x.Content4 };

            MessageDataReader = new Func<MessageRecord, byte[]>[] { (x) => x.Data0, (x) => x.Data1, (x) => x.Data2, (x) => x.Data3, (x) => x.Data4 };

            SagaDataWriter = new Action<SagaRecord, byte[]>[] { (x, y) => x.Data0 = y, (x, y) => x.Data1 = y, (x, y) => x.Data2 = y, (x, y) => x.Data3 = y, (x, y) => x.Data4 = y };

            SagaDataReader = new Func<SagaRecord, byte[]>[] { (x) => x.Data0, (x) => x.Data1, (x) => x.Data2, (x) => x.Data3, (x) => x.Data4 };
        }

        private static CloudTable GetCloudTable(string connectionstring, string tablename)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tablename);

            return table;
        }

        public override SagaEntity[] GetSagaEntities(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

            if (!string.IsNullOrWhiteSpace(sagastoragename))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{sagastoragename}");
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

            var where = TableQuery.CombineFilters(
                            TableQuery.CombineFilters(
                                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey),
                                TableOperators.And,
                                TableQuery.GenerateFilterConditionForDate("Created", QueryComparisons.GreaterThanOrEqual, start)),
                        TableOperators.And,
                        TableQuery.GenerateFilterConditionForDate("Created", QueryComparisons.LessThan, end));

            var query = new TableQuery<SagaRecord>().Where(where);

            var records = ExecuteQuery<SagaRecord>(table, query);

            return records.Select(record =>
            {
                var entity = new SagaEntity()
                {
                    DataType = record.DataType,
                    Name = record.Name,
                    Created = record.Created,
                    Updated = record.Updated,
                    Ended = record.Ended,
                    Timeout = record.Timeout,
                    Status = record.Status,
                    Duration = record.Duration,
                    Id = record.Id
                };

                ReadSaga(record, entity);

                return entity;
            }
            ).ToArray();
        }

        private void ReadSaga(SagaRecord record, SagaEntity entity)
        {
            if (record.NumberOfDataArrays > 0)
            {
                var bytearray = Combine(record.NumberOfDataArrays, i => SagaDataReader[i].Invoke(record));

                entity.Data = _parameter.TableStorageStringEncoding.GetString(bytearray);
            }
            else
            {
                entity.Data = record.Data;
            }
        }

        public static IEnumerable<T> ExecuteQuery<T>(CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
        {
            TableContinuationToken token = null;
            var retVal = new List<T>();
            do
            {
                var results = table.ExecuteQuerySegmentedAsync(query, token).GetAwaiter().GetResult();
                retVal.AddRange(results.Results);
                token = results.ContinuationToken;
            } while (token != null);

            return retVal;
        }

        public override MessageEntity[] GetMessageEntitiesBySagaEntity(SagaEntity sagaentity, string messagestoragename = "")
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

            if (!string.IsNullOrWhiteSpace(messagestoragename))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{messagestoragename}");
            }

            var key = GetRowKey(sagaentity.Id);

            var where = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key);

            var query = new TableQuery<MessageRecord>().Where(where);

            var records = ExecuteQuery<MessageRecord>(table, query);

            return records.Select(record =>
            {
                var entity = new MessageEntity()
                {
                    ContentType = record.ContentType,
                    Identity = string.IsNullOrWhiteSpace(record.Identity) ? null : serializer.Deserialize<IdentityContext>(record.Identity),
                    Version = record.Version,
                    RetryCount = record.RetryCount,
                    LastRetry = record.LastRetry,
                    Origin = string.IsNullOrWhiteSpace(record.Origin) ? null : serializer.Deserialize<Origin>(record.Origin),
                    Saga = string.IsNullOrWhiteSpace(record.Saga) ? null : serializer.Deserialize<SagaContext>(record.Saga),
                    Headers = string.IsNullOrWhiteSpace(record.Headers) ? null : serializer.Deserialize<Dictionary<string, string>>(record.Headers),
                    DateTimeUtc = record.DateTimeUtc,
                    Name = record.Name,
                    ContentId = record.ContentId,
                    Type = !string.IsNullOrWhiteSpace(record.Type) ? (MessageEntityType)Enum.Parse(typeof(MessageEntityType), record.Type) : MessageEntityType.Inbound,
                    SagaId = record.SagaId,
                    Id = record.Id
                };

                ReadMessage(record, entity);

                return entity;
            }).ToArray();
        }

        private void ReadMessage(MessageRecord record, MessageEntity entity)
        {
            if (record.NumberOfDataArrays > 0)
            {
                var bytearray = Combine(record.NumberOfDataArrays, i => MessageDataReader[i].Invoke(record));

                entity.Data = _parameter.TableStorageStringEncoding.GetString(bytearray);
            }
            else
            {
                entity.Data = record.Data;
            }

            if (record.NumberOfContentArrays > 0)
            {
                var bytearray = Combine(record.NumberOfContentArrays, i => MessageContentReader[i].Invoke(record));

                entity.Content = _parameter.TableStorageStringEncoding.GetString(bytearray);
            }
            else
            {
                entity.Content = record.Content;
            }
        }

        public override MessageEntity[] GetMessageEntities(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

            if (!string.IsNullOrWhiteSpace(messagestoragename))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{messagestoragename}");
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
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var where = TableQuery.CombineFilters(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("DateTimeUtc", QueryComparisons.GreaterThanOrEqual, start)),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForDate("DateTimeUtc", QueryComparisons.LessThan, end));

            var query = new TableQuery<MessageRecord>().Where(where);

            var records = ExecuteQuery<MessageRecord>(table, query);

            return records.Select(record => {
                var entity = new MessageEntity()
                {
                    ContentType = record.ContentType,
                    Identity = !string.IsNullOrWhiteSpace(record.Identity) ? serializer.Deserialize<IdentityContext>(record.Identity) : null,
                    Version = record.Version,
                    RetryCount = record.RetryCount,
                    LastRetry = record.LastRetry,
                    Origin = !string.IsNullOrWhiteSpace(record.Origin) ? serializer.Deserialize<Origin>(record.Origin) : null,
                    Saga = !string.IsNullOrWhiteSpace(record.Saga) ? serializer.Deserialize<SagaContext>(record.Saga) : null,
                    Headers = !string.IsNullOrWhiteSpace(record.Headers) ? serializer.Deserialize<Dictionary<string, string>>(record.Headers) : null,
                    Tracks = !string.IsNullOrWhiteSpace(record.Tracks) ? serializer.Deserialize<List<Track>>(record.Tracks) : null,
                    DateTimeUtc = record.DateTimeUtc,
                    Name = record.Name,
                    ContentId = record.ContentId,
                    SagaId = record.SagaId,
                    Id = record.Id,
                    Type = !string.IsNullOrWhiteSpace(record.Type) ? (MessageEntityType)Enum.Parse(typeof(MessageEntityType), record.Type): MessageEntityType.Inbound
                };

                ReadMessage(record, entity);

                return entity;

            }).ToArray();
        }

        private void WriteMessage(MessageEntity messageentity, MessageRecord record)
        {
            var bytescount = _parameter.TableStorageStringEncoding.GetByteCount(messageentity.Content);
            record.SizeOfContent = bytescount;
            if (bytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
            {
                record.Content = messageentity.Content;
            }
            else
            {
                record.SizeOfContentArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                record.NumberOfContentArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(messageentity.Content), bytescount, (i, buffer) => MessageContentWriter[i].Invoke(record, buffer));
            }

            if(messageentity.Data!=null)
            {
                var databytescount = _parameter.TableStorageStringEncoding.GetByteCount(messageentity.Data);
                record.SizeOfData = databytescount;
                if (databytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
                {
                    record.Data = messageentity.Data;
                }
                else
                {
                    record.SizeOfDataArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                    record.NumberOfDataArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(messageentity.Data), databytescount, (i, buffer) => MessageDataWriter[i].Invoke(record, buffer));
                }
            }
        }

        public override async Task<SagaEntity> GetSagaEntity(string entityId)
        {
            try
            {
                var entity = new SagaEntity();

                var tablenamesufix = GetTableNameSufix(entityId);

                var partitionkey = GetPartitionKey(entityId);

                var rowkey = GetRowKey(entityId);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = await table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey)).ConfigureAwait(false);

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        entity.DataType = record.DataType;

                        entity.Name = record.Name;

                        entity.Created = record.Created;

                        entity.Updated = record.Updated;

                        entity.Ended = record.Ended;

                        entity.Timeout = record.Timeout;

                        entity.Status = record.Status;

                        entity.Duration = record.Duration;

                        entity.Id = record.Id;

                        ReadSaga(record, entity);

                        return entity;
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga key {entityId}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga key format {entityId}");
                }
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga record query for saga key {entityId}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record query for saga key {entityId}", ex);
            }
        }

        public override async Task<SagaEntity> CreateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            try
            {
                var rowkey = Guid.NewGuid().ToString();

                var partition = $"{sagaentity.Created.ToString("yyyyMMdd")}_{sagaentity.Name}";

                var id = $"{partition}@{rowkey}@{_parameter.TableSufix}";

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

                var record = new SagaRecord(partition, rowkey)
                {
                    Created = sagaentity.Created,

                    Updated = sagaentity.Updated,

                    Name = sagaentity.Name,

                    DataType = sagaentity.DataType,

                    Timeout = sagaentity.Timeout,

                    Status = sagaentity.Status,

                    Id = id
                };

                sagaentity.Id = id;

                WriteSaga(sagaentity, record);

                var result = await table.ExecuteAsync(TableOperation.Insert(record)).ConfigureAwait(false);

                return sagaentity;
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {sagaentity.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {sagaentity.Name} and route {context.Route.Name}", ex);
            }
        }

        private void WriteSaga(SagaEntity sagaentity, SagaRecord record)
        {
            var databytescount = _parameter.TableStorageStringEncoding.GetByteCount(sagaentity.Data);
            record.SizeOfData = databytescount;
            if (databytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
            {
                record.Data = sagaentity.Data;
            }
            else
            {
                record.SizeOfDataArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                record.NumberOfDataArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(sagaentity.Data), databytescount, (i, buffer) => SagaDataWriter[i].Invoke(record, buffer));
            }
        }

        public override async Task<MessageEntity> CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {
            try
            {
                var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{messageentity.Name}";

                var rowkey = $"{Guid.NewGuid()}";

                if(!string.IsNullOrWhiteSpace(messageentity.SagaId))
                {
                    partition = GetRowKey(messageentity.SagaId);

                    rowkey = $"{messageentity.Name}_{Guid.NewGuid()}";
                }

                var id = $"{partition}@{rowkey}@{_parameter.TableSufix}";

                messageentity.Id = id;

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

                var record = new MessageRecord(partition, $"{Guid.NewGuid()}")
                {
                    ContentType = messageentity.ContentType,
                    Identity = serializer.Serialize(messageentity.Identity),
                    Version = messageentity.Version,
                    RetryCount = messageentity.RetryCount,
                    LastRetry = messageentity.LastRetry,
                    Origin = serializer.Serialize(messageentity.Origin),
                    Headers = serializer.Serialize(messageentity.Headers),
                    DateTimeUtc = messageentity.DateTimeUtc,
                    Name = messageentity.Name,
                    Tracks = serializer.Serialize(messageentity.Tracks),
                    ContentId = messageentity.ContentId,
                    Type = messageentity.Type.ToString(),
                    Saga = serializer.Serialize(messageentity.Saga),
                    Id = messageentity.Id,
                    SagaId=messageentity.SagaId
                };

                WriteMessage(messageentity, record);

                var result = await table.ExecuteAsync(TableOperation.Insert(record)).ConfigureAwait(false);

                return messageentity;
            }
            catch(StorageException s)
            {
                throw new ApplicationException($"Error during the message record creation with content {messageentity.ContentType} and route/endpoint {messageentity.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation with content {messageentity.ContentType} and route {messageentity.Name}", ex);
            }
        }

        private int Split(byte[] contentinbytes, int bytescount, Action<int, byte[]> action)
        {
            var maxbytescount = _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte;

            var chunks = (int)Math.Ceiling((decimal)bytescount / maxbytescount);

            var remainingbytescount = bytescount;

            for (int i = 0; i < chunks; i++)
            {
                var buffersize = maxbytescount;

                if (remainingbytescount < maxbytescount)
                {
                    buffersize = remainingbytescount;
                }

                var buffer = new byte[buffersize];

                Buffer.BlockCopy(contentinbytes, maxbytescount * i, buffer, 0, buffersize);

                remainingbytescount = remainingbytescount - maxbytescount;

                action(i, buffer);
            }

            return chunks;
        }

        private byte[] Combine(int chunks, Func<int, byte[]> func)
        {
            var lenght = 0;

            for (int i = 0; i < chunks; i++)
            {
                lenght = lenght + func(i).Length;
            }

            var rv = new byte[lenght];

            int offset = 0;

            for (int i = 0; i < chunks; i++)
            {
                var array = func(i);

                Buffer.BlockCopy(array, 0, rv, offset, array.Length);

                offset += array.Length;
            }

            return rv;
        }

        public override async Task UpdateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            try
            {
                var tablenamesufix = GetTableNameSufix(sagaentity.Id);

                var partitionkey = GetPartitionKey(sagaentity.Id);

                var rowkey = GetRowKey(sagaentity.Id);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey)).GetAwaiter().GetResult();

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        record.Updated = sagaentity.Updated;

                        if (!string.IsNullOrWhiteSpace(sagaentity.Status))
                        {
                            record.Status = sagaentity.Status;
                        }

                        record.Duration = sagaentity.Duration;

                        if (sagaentity.Ended != null)
                        {
                            record.Ended = sagaentity.Ended;
                        }

                        record.ETag = "*";

                        record.Data = null;

                        record.Data0 = null;

                        record.Data1 = null;

                        record.Data2 = null;

                        record.Data3 = null;

                        record.Data4 = null;

                        record.NumberOfDataArrays = 0;

                        record.SizeOfDataArraysOnKilobytes = 0;

                        WriteSaga(sagaentity, record);

                        await table.ExecuteAsync(TableOperation.Replace(record)).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga id {sagaentity.Id}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga id format {sagaentity.Id}");
                }
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga update for saga {sagaentity.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga update for saga {sagaentity.Name} and route {context.Route.Name}", ex);
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
