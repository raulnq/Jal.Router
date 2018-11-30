﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Inbound.Sagas;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureSagaStorage : AbstractSagaStorage
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly AzureStorageParameter _parameter;

        private readonly int _kilobyte = 1024;

        private readonly Action<MessageRecord, byte[]>[] MessageContentFiller;

        private readonly Action<MessageRecord, byte[]>[] MessageDataFiller;

        public AzureSagaStorage(IComponentFactory factory, IConfiguration configuration, IParameterProvider provider)
        {
            _factory = factory;

            _configuration = configuration;

            _parameter = provider.Get<AzureStorageParameter>();

            MessageContentFiller = new Action<MessageRecord, byte[]>[] { (x, y) => x.Content0 = y, (x, y) => x.Content1 = y , (x, y) => x.Content2 = y , (x, y) => x.Content3 = y , (x, y) => x.Content4 = y };

            MessageDataFiller = new Action<MessageRecord, byte[]>[] { (x, y) => x.Data0 = y, (x, y) => x.Data1 = y, (x, y) => x.Data2 = y, (x, y) => x.Data3 = y, (x, y) => x.Data4 = y };
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

        public override MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "")
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

            if (!string.IsNullOrWhiteSpace(messagestoragename))
            {
                table = GetCloudTable(_parameter.TableStorageConnectionString, $"{messagestoragename}");
            }

            var where = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sagaentity.Key);

            var query = new TableQuery<MessageRecord>().Where(where);

            var records = ExecuteQuery<MessageRecord>(table, query);

            return records.Select(x => new MessageEntity()
            {
                Content = x.Content,
                ContentType = x.ContentType,
                Identity = string.IsNullOrWhiteSpace(x.Identity) ? null: serializer.Deserialize<Identity>(x.Identity),
                Version = x.Version,
                RetryCount = x.RetryCount,
                LastRetry = x.LastRetry,
                Origin = string.IsNullOrWhiteSpace(x.Origin) ? null : serializer.Deserialize<Origin>(x.Origin),
                Saga = string.IsNullOrWhiteSpace(x.Saga) ? null : serializer.Deserialize<SagaContext>(x.Saga),
                Headers = string.IsNullOrWhiteSpace(x.Headers) ? null : serializer.Deserialize<Dictionary<string, string>>(x.Headers),
                DateTimeUtc = x.DateTimeUtc,
                Data = x.Data,
                Name = x.Name,
                ContentId = x.ContentId
            }).ToArray();
        }

        public override MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
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

            return records.Select(x => new MessageEntity()
            {
                Content = x.Content,
                ContentType = x.ContentType,
                Identity = !string.IsNullOrWhiteSpace(x.Identity) ? serializer.Deserialize<Identity>(x.Identity) : null,
                Version = x.Version,
                RetryCount = x.RetryCount,
                LastRetry = x.LastRetry,
                Origin = !string.IsNullOrWhiteSpace(x.Origin) ? serializer.Deserialize<Origin>(x.Origin) : null,
                Saga = !string.IsNullOrWhiteSpace(x.Saga) ? serializer.Deserialize<SagaContext>(x.Saga) : null,
                Headers = !string.IsNullOrWhiteSpace(x.Headers) ? serializer.Deserialize<Dictionary<string, string>>(x.Headers) : null,
                Tracks = !string.IsNullOrWhiteSpace(x.Tracks) ? serializer.Deserialize<List<Track>>(x.Tracks) : null,
                DateTimeUtc = x.DateTimeUtc,
                Data = x.Data,
                Name = x.Name,
                ContentId = x.ContentId,
            }).ToArray();
        }

        public override void CreateMessage(MessageContext context, string sagaid, SagaEntity sagaentity, MessageEntity messageentity)
        {
            try
            {
                var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

                var tablenamesufix = GetTableNameSufix(sagaid);

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{tablenamesufix}");

                var record = new MessageRecord(sagaentity.Key, $"{messageentity.Name}_{Guid.NewGuid()}")
                {
                    Content = messageentity.Content,
                    ContentType = messageentity.ContentType,
                    Identity = serializer.Serialize(messageentity.Identity),
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
                    ContentId = messageentity.ContentId,
                };

                var size = Encoding.UTF8.GetByteCount(record.Content);

                if (size >= 64000)
                {
                    record.Content = LimitByteLength(record.Content, 63000) + "...";
                }

                var result = table.ExecuteAsync(TableOperation.Insert(record)).GetAwaiter().GetResult();
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the message record creation for saga {context.Saga.Name} with content {context.ContentType.FullName} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation for saga {context.Saga.Name} with content {context.ContentType.FullName} and route {context.Route.Name}", ex);
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

        public override SagaEntity GetSaga(string sagaid)
        {
            try
            {
                var entity = new SagaEntity();

                var tablenamesufix = GetTableNameSufix(sagaid);

                var partitionkey = GetPartitionKey(sagaid);

                var rowkey = GetRowKey(sagaid);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey)).GetAwaiter().GetResult();

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
                        throw new ApplicationException($"Record not found for saga key {sagaid}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga key format {sagaid}");
                }
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga record query for saga key {sagaid}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record query for saga key {sagaid}", ex);
            }
        }

        public override string CreateSaga(MessageContext context, SagaEntity sagaentity)
        {
            try
            {
                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{context.Saga.Name}";

                var id = $"{partition}@{sagaentity.Key}@{_parameter.TableSufix}";
                
                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

                var record = new SagaRecord(partition, sagaentity.Key)
                {
                    Data = sagaentity.Data,
                    Created = sagaentity.Created,
                    Updated = sagaentity.Updated,
                    Name = sagaentity.Name,
                    DataType = sagaentity.DataType,
                    Timeout = sagaentity.Timeout,
                    Status = sagaentity.Status
                };

                var result = table.ExecuteAsync(TableOperation.Insert(record)).GetAwaiter().GetResult();

                return id;
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {context.Saga.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga record creation for saga {context.Saga.Name} and route {context.Route.Name}", ex);
            }
        }

        public override void CreateMessage(MessageContext context, MessageEntity messageentity)
        {
            try
            {
                var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

                var partition = $"{context.DateTimeUtc.ToString("yyyyMMdd")}_{context.Route.Name}";

                var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

                var bytescount = _parameter.TableStorageStringEncoding.GetByteCount(messageentity.Content);

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
                    Data = messageentity.Data,
                };

                if (bytescount < _parameter.TableStorageMaxColumnSizeOnKilobytes * _kilobyte)
                {
                    record.Content = messageentity.Content;
                }
                else
                {
                    record.SizeOfContentArraysOnKilobytes = _parameter.TableStorageMaxColumnSizeOnKilobytes;
                    record.NumberOfContentArrays = PopulateArrays(_parameter.TableStorageStringEncoding.GetBytes(messageentity.Content), bytescount, (i, buffer) => MessageContentFiller[i].Invoke(record, buffer));
                }

                var result = table.ExecuteAsync(TableOperation.Insert(record)).GetAwaiter().GetResult();
            }
            catch(StorageException s)
            {
                throw new ApplicationException($"Error during the message record creation with content {context.ContentType.FullName} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the message record creation with content {context.ContentType.FullName} and route {context.Route.Name}", ex);
            }
        }

        private int PopulateArrays(byte[] contentinbytes, int bytescount, Action<int, byte[]> action)
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

        public override void UpdateSaga(MessageContext context, string sagaid, SagaEntity sagaentity)
        {
            try
            {
                var tablenamesufix = GetTableNameSufix(sagaid);

                var partitionkey = GetPartitionKey(sagaid);

                var rowkey = GetRowKey(sagaid);

                if (!string.IsNullOrWhiteSpace(tablenamesufix) && !string.IsNullOrWhiteSpace(partitionkey) && !string.IsNullOrWhiteSpace(rowkey))
                {
                    var table = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{tablenamesufix}");

                    var result = table.ExecuteAsync(TableOperation.Retrieve<SagaRecord>(partitionkey, rowkey)).GetAwaiter().GetResult();

                    var record = result.Result as SagaRecord;

                    if (record != null)
                    {
                        record.Data = sagaentity.Data;

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

                        table.ExecuteAsync(TableOperation.Replace(record)).GetAwaiter().GetResult();
                    }
                    else
                    {
                        throw new ApplicationException($"Record not found for saga id {sagaid}");
                    }
                }
                else
                {
                    throw new ApplicationException($"Invalid saga id format {sagaid}");
                }
            }
            catch (StorageException s)
            {
                throw new ApplicationException($"Error during the saga update for saga {context.Route.Name} and route {context.Route.Name}, status code: {s.RequestInformation?.HttpStatusCode} error code: {s.RequestInformation?.ExtendedErrorInformation?.ErrorCode} error message: {s.RequestInformation?.ExtendedErrorInformation?.ErrorMessage}", s);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the saga update for saga {context.Route.Name} and route {context.Route.Name}", ex);
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
