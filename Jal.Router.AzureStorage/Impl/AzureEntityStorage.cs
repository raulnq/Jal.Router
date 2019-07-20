using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureEntityStorage : AbstractEntityStorage
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly IConfiguration _configuration;

        private readonly AzureStorageParameter _parameter;

        private readonly int _kilobyte = 1024;

        private readonly Action<MessageRecord, byte[]>[] MessageContentWriter;

        private readonly Action<MessageRecord, byte[]>[] MessageSagaWriter;

        private readonly Action<SagaRecord, byte[]>[] SagaDataWriter;

        private readonly Func<SagaRecord, byte[]>[] SagaDataReader;

        private readonly Func<MessageRecord, byte[]>[] MessageContentReader;

        private readonly Func<MessageRecord, byte[]>[] MessageSagaReader;

        public AzureEntityStorage(IComponentFactoryGateway factory, IConfiguration configuration, IParameterProvider provider)
        {
            _factory = factory;

            _configuration = configuration;

            _parameter = provider.Get<AzureStorageParameter>();

            MessageContentWriter = new Action<MessageRecord, byte[]>[] { (x, y) => x.ContentContextEntity0 = y, (x, y) => x.ContentContextEntity1 = y , (x, y) => x.ContentContextEntity2 = y , (x, y) => x.ContentContextEntity3 = y , (x, y) => x.ContentContextEntity4 = y };

            MessageSagaWriter = new Action<MessageRecord, byte[]>[] { (x, y) => x.SagaContextEntity0 = y, (x, y) => x.SagaContextEntity1 = y, (x, y) => x.SagaContextEntity2 = y, (x, y) => x.SagaContextEntity3 = y, (x, y) => x.SagaContextEntity4 = y };

            MessageContentReader = new Func<MessageRecord, byte[]>[] { (x) => x.ContentContextEntity0, (x) => x.ContentContextEntity1, (x) => x.ContentContextEntity2, (x) => x.ContentContextEntity3, (x) => x.ContentContextEntity4 };

            MessageSagaReader = new Func<MessageRecord, byte[]>[] { (x) => x.SagaContextEntity0, (x) => x.SagaContextEntity1, (x) => x.SagaContextEntity2, (x) => x.SagaContextEntity3, (x) => x.SagaContextEntity4 };

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

        public override async Task<SagaData[]> GetSagaData(DateTime start, DateTime end, string saganame, IDictionary<string, string> options = null)
        {
            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

            if (options!=null && options.ContainsKey("sagastoragename") && !string.IsNullOrWhiteSpace(options["sagastoragename"]))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{options["sagastoragename"]}");
            }

            var diff = (end - start).TotalDays;

            var currentdate = new DateTime(start.Year, start.Month, start.Day);

            var list = new List<SagaData>();

            for (var i = 0; i <= diff; i++)
            {
                var partitionkey = $"{currentdate.ToString("yyyyMMdd")}_{saganame}";

                var sagas = await GetSagaData(table, partitionkey, start, end, options).ConfigureAwait(false);

                list.AddRange(sagas);

                currentdate =currentdate.AddDays(1);
            }

            return list.ToArray();
        }

        private async Task<SagaData[]> GetSagaData(CloudTable table, string partitionkey, DateTime start, DateTime end, IDictionary<string, string> options = null)
        {

            var where = TableQuery.CombineFilters(
                            TableQuery.CombineFilters(
                                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey),
                                TableOperators.And,
                                TableQuery.GenerateFilterConditionForDate("Created", QueryComparisons.GreaterThanOrEqual, start)),
                        TableOperators.And,
                        TableQuery.GenerateFilterConditionForDate("Created", QueryComparisons.LessThan, end));

            var query = new TableQuery<SagaRecord>().Where(where);

            var records = await ExecuteQuery<SagaRecord>(table, query).ConfigureAwait(false);

            var serializer = _factory.CreateMessageSerializer();

            return records.Select(record =>
            {
                var sagatype = serializer.Deserialize<Type>(record.DataType);

                var data = ReadSaga(record, sagatype);

                var entity = new SagaData(record.Id, data, sagatype, record.Name, record.Created, record.Timeout, record.Status, record.Updated, record.Ended, record.Duration);

                return entity;
            }
            ).ToArray();
        }

        private object ReadSaga(SagaRecord record, Type sagatype)
        {
            var serializer = _factory.CreateMessageSerializer();

            if (record.NumberOfDataArrays > 0)
            {
                var bytearray = Combine(record.NumberOfDataArrays, i => SagaDataReader[i].Invoke(record));

                var data = _parameter.TableStorageStringEncoding.GetString(bytearray);

                return serializer.Deserialize(data, sagatype);
            }
            else
            {
                return serializer.Deserialize(record.Data, sagatype);
            }
        }

        public static async Task<IEnumerable<T>> ExecuteQuery<T>(CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
        {
            TableContinuationToken token = null;
            var retVal = new List<T>();
            do
            {
                var results = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                retVal.AddRange(results.Results);
                token = results.ContinuationToken;
            } while (token != null);

            return retVal;
        }

        public override async Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IDictionary<string, string> options = null)
        {
            var serializer = _factory.CreateMessageSerializer();

            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

            if (options != null && options.ContainsKey("messagestoragename") && !string.IsNullOrWhiteSpace(options["messagestoragename"]))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{options["messagestoragename"]}");
            }

            var key = GetRowKey(sagadata.Id);

            var where = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key);

            var query = new TableQuery<MessageRecord>().Where(where);

            var records = await ExecuteQuery(table, query).ConfigureAwait(false);

            return records.Select(record =>
            {
                return RecordToEntity(record, serializer);
            }).ToArray();
        }

        private ContentContextEntity ReadContentContextEntity(MessageRecord record)
        {
            var serializer = _factory.CreateMessageSerializer();

            if (record.NumberOfContentArrays > 0)
            {
                var bytearray = Combine(record.NumberOfContentArrays, i => MessageContentReader[i].Invoke(record));

                var content = _parameter.TableStorageStringEncoding.GetString(bytearray);

                return serializer.Deserialize<ContentContextEntity>(content);
            }
            else
            {
                return serializer.Deserialize<ContentContextEntity>(record.ContentContextEntity);
            }
        }

        private SagaContextEntity ReadSagaContextEntity(MessageRecord record)
        {
            var serializer = _factory.CreateMessageSerializer();

            if (record.NumberOfSagaArrays > 0)
            {
                var bytearray = Combine(record.NumberOfSagaArrays, i => MessageSagaReader[i].Invoke(record));

                var saga = _parameter.TableStorageStringEncoding.GetString(bytearray);

                return serializer.Deserialize<SagaContextEntity>(saga);
            }
            else
            {
                return serializer.Deserialize<SagaContextEntity>(record.SagaContextEntity);
            }
        }

        public override async Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null)
        {
            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

            if (options != null && options.ContainsKey("messagestoragename") && !string.IsNullOrWhiteSpace(options["messagestoragename"]))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{options["messagestoragename"]}");
            }

            var diff = (int)(end - start).TotalDays;

            var currentdate = new DateTime(start.Year, start.Month, start.Day);

            var list = new List<MessageEntity>();

            for (var i = 0; i <= diff; i++)
            {
                var partitionkey = $"{currentdate.ToString("yyyyMMdd")}_{routename}";

                var messages = await GetMessages(table, partitionkey, start, end).ConfigureAwait(false);

                list.AddRange(messages);

                currentdate = currentdate.AddDays(1);
            }

            return list.ToArray();
        }

        private async Task<MessageEntity[]> GetMessages(CloudTable table, string partitionkey, DateTime start, DateTime end)
        {
            var serializer = _factory.CreateMessageSerializer();

            var where = TableQuery.CombineFilters(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("DateTimeUtc", QueryComparisons.GreaterThanOrEqual, start)),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForDate("DateTimeUtc", QueryComparisons.LessThan, end));

            var query = new TableQuery<MessageRecord>().Where(where);

            var records = await ExecuteQuery(table, query).ConfigureAwait(false);

            return records.Select(record =>
            {
                return RecordToEntity(record, serializer);
            }).ToArray();
        }

        private MessageEntity RecordToEntity(MessageRecord record, IMessageSerializer serializer)
        {
            var identity = !string.IsNullOrWhiteSpace(record.IdentityContextEntity) ? serializer.Deserialize<IdentityContextEntity>(record.IdentityContextEntity) : null;
            var channel = !string.IsNullOrWhiteSpace(record.ChannelEntity) ? serializer.Deserialize<ChannelEntity>(record.ChannelEntity) : null;
            var headers = !string.IsNullOrWhiteSpace(record.Headers) ? serializer.Deserialize<Dictionary<string, string>>(record.Headers) : null;
            var router = !string.IsNullOrWhiteSpace(record.RouteEntity) ? serializer.Deserialize<RouteEntity>(record.RouteEntity) : null;
            var endpoint = !string.IsNullOrWhiteSpace(record.EndpointEntity) ? serializer.Deserialize<EndpointEntity>(record.EndpointEntity) : null;
            var origin = !string.IsNullOrWhiteSpace(record.OriginEntity) ? serializer.Deserialize<OriginEntity>(record.OriginEntity) : null;
            var saga = !string.IsNullOrWhiteSpace(record.SagaEntity) ? serializer.Deserialize<SagaEntity>(record.SagaEntity) : null;
            var tracking = !string.IsNullOrWhiteSpace(record.TrackingContextEntity) ? serializer.Deserialize<TrackingContextEntity>(record.TrackingContextEntity) : null;
            var contentcontext = ReadContentContextEntity(record);
            var sagacontext = ReadSagaContextEntity(record);
            var entity = new MessageEntity(record.Id, record.Host, channel, headers, record.Version, record.DateTimeUtc, record.ScheduledEnqueueDateTimeUtc, router, endpoint,
                origin, saga, contentcontext, sagacontext, tracking, identity, record.Name);
            return entity;
        }

        private void WriteMessage(MessageEntity messageentity, MessageRecord record)
        {
            var serializer = _factory.CreateMessageSerializer();
            var content = serializer.Serialize(messageentity.ContentContextEntity);
            var bytescount = _parameter.TableStorageStringEncoding.GetByteCount(content);
            record.SizeOfContent = bytescount;
            if (bytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
            {
                record.ContentContextEntity = content;
            }
            else
            {
                record.SizeOfContentArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                record.NumberOfContentArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(content), bytescount, (i, buffer) => MessageContentWriter[i].Invoke(record, buffer));
            }

            if(messageentity.SagaContextEntity != null)
            {
                var saga = serializer.Serialize(messageentity.SagaContextEntity);

                var sagabytescount = _parameter.TableStorageStringEncoding.GetByteCount(saga);
                record.SizeOfSaga = sagabytescount;
                if (sagabytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
                {
                    record.SagaContextEntity = saga;
                }
                else
                {
                    record.SizeOfSagaArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                    record.NumberOfSagaArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(saga), sagabytescount, (i, buffer) => MessageSagaWriter[i].Invoke(record, buffer));
                }
            }
        }

        public override async Task<SagaData> GetSagaData(string entityId)
        {
            try
            {
                

                var tablenamesufix = GetTableNameSufix(entityId);

                var partitionkey = GetPartitionKey(entityId);

                var rowkey = GetRowKey(entityId);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = await table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey)).ConfigureAwait(false);

                    var record = result.Result as SagaRecord;

                    var serializer = _factory.CreateMessageSerializer();

                    if (record != null)
                    {
                        var sagatype = serializer.Deserialize<Type>(record.DataType);

                        var data = ReadSaga(record, sagatype);

                        var entity = new SagaData(record.Id, data,  sagatype, record.Name, record.Created, record.Timeout, record.Status, record.Updated, record.Ended, record.Duration);

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

        public override async Task CreateSagaData(MessageContext context, SagaData sagadata)
        {
            try
            {
                var rowkey = Guid.NewGuid().ToString();

                var partition = $"{sagadata.Created.ToString("yyyyMMdd")}_{sagadata.Name}";

                var id = $"{partition}@{rowkey}@{_parameter.TableSufix}";

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

                var serializer = _factory.CreateMessageSerializer();

                var record = new SagaRecord(partition, rowkey)
                {
                    Created = sagadata.Created,

                    Updated = sagadata.Updated,

                    Name = sagadata.Name,

                    DataType = serializer.Serialize(sagadata.DataType),

                    Timeout = sagadata.Timeout,

                    Status = sagadata.Status,

                    Id = id
                };

                sagadata.UpdateId(id);

                WriteSaga(sagadata, record);

                await table.ExecuteAsync(TableOperation.Insert(record)).ConfigureAwait(false);
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {sagadata.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {sagadata.Name} and route {context.Route.Name}", ex);
            }
        }

        private void WriteSaga(SagaData sagadata, SagaRecord record)
        {
            var serializer = _factory.CreateMessageSerializer();

            var data = serializer.Serialize(sagadata.Data);

            var databytescount = _parameter.TableStorageStringEncoding.GetByteCount(data);
            record.SizeOfData = databytescount;
            if (databytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
            {
                record.Data = data;
            }
            else
            {
                record.SizeOfDataArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                record.NumberOfDataArrays = Split(_parameter.TableStorageStringEncoding.GetBytes(data), databytescount, (i, buffer) => SagaDataWriter[i].Invoke(record, buffer));
            }
        }

        public override async Task CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {
            try
            {
                var serializer = _factory.CreateMessageSerializer();

                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{messageentity.Name}";

                var rowkey = $"{Guid.NewGuid()}";

                if(!string.IsNullOrWhiteSpace(messageentity.SagaContextEntity?.SagaData?.Id))
                {
                    partition = GetRowKey(messageentity.SagaContextEntity.SagaData.Id);

                    rowkey = $"{messageentity.Name}_{Guid.NewGuid()}";
                }

                var id = $"{partition}@{rowkey}@{_parameter.TableSufix}";

                messageentity.UpdateId(id);

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

                var record = new MessageRecord(partition, $"{Guid.NewGuid()}")
                {
                    Host = messageentity.Host,
                    IdentityContextEntity = messageentity.IdentityContextEntity!=null?serializer.Serialize(messageentity.IdentityContextEntity):null,
                    Version = messageentity.Version,
                    OriginEntity = messageentity.OriginEntity!=null?serializer.Serialize(messageentity.OriginEntity):null,
                    Headers = messageentity.Headers!=null?serializer.Serialize(messageentity.Headers):null,
                    DateTimeUtc = messageentity.DateTimeUtc,
                    Name = messageentity.Name,
                    TrackingContextEntity = messageentity.TrackingContextEntity!=null?serializer.Serialize(messageentity.TrackingContextEntity):null,
                    ChannelEntity = messageentity.ChannelEntity!=null?serializer.Serialize(messageentity.ChannelEntity):null,
                    Id = messageentity.Id,
                    ScheduledEnqueueDateTimeUtc = messageentity.ScheduledEnqueueDateTimeUtc,
                    EndpointEntity = messageentity.EndpointEntity!=null?serializer.Serialize(messageentity.EndpointEntity):null,
                    RouteEntity = messageentity.RouteEntity!=null?serializer.Serialize(messageentity.RouteEntity):null,
                    SagaEntity = messageentity.SagaEntity!=null?serializer.Serialize(messageentity.SagaEntity):null
                };

                WriteMessage(messageentity, record);

                await table.ExecuteAsync(TableOperation.Insert(record)).ConfigureAwait(false);
            }
            catch(StorageException s)
            {
                throw new ApplicationException($"Error during the message record creation with content {messageentity.ContentContextEntity.Type} and route/endpoint {messageentity.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation with content {messageentity.ContentContextEntity.Type} and route {messageentity.Name}", ex);
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

        public override async Task UpdateSagaData(MessageContext context, SagaData sagadata)
        {
            try
            {
                var tablenamesufix = GetTableNameSufix(sagadata.Id);

                var partitionkey = GetPartitionKey(sagadata.Id);

                var rowkey = GetRowKey(sagadata.Id);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = await table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey));

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        record.Updated = sagadata.Updated;

                        if (!string.IsNullOrWhiteSpace(sagadata.Status))
                        {
                            record.Status = sagadata.Status;
                        }

                        record.Duration = sagadata.Duration;

                        if (sagadata.Ended != null)
                        {
                            record.Ended = sagadata.Ended;
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

                        WriteSaga(sagadata, record);

                        await table.ExecuteAsync(TableOperation.Replace(record)).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga id {sagadata.Id}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga id format {sagadata.Id}");
                }
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga update for saga {sagadata.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga update for saga {sagadata.Name} and route {context.Route.Name}", ex);
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
