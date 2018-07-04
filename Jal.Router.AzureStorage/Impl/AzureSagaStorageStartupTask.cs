using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureSagaStorageStartupTask : IStartupTask
    {
        private readonly string _connectionstring;

        private readonly string _sagastoragename;

        private readonly string _messagestorgename;

        private readonly string _tablenamesufix;

        private readonly ILogger _logger;

        public AzureSagaStorageStartupTask(ILogger logger, string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            _connectionstring = connectionstring;

            _tablenamesufix = tablenamesufix;

            _sagastoragename = sagastoragename;

            _messagestorgename = messagestorgename;

            _logger = logger;
        }

        private static CloudTable GetCloudTable(string connectionstring, string tablename)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tablename);

            return table;
        }

        public void Run()
        {
            var sagatable = GetCloudTable(_connectionstring, $"{_sagastoragename}{_tablenamesufix}");

            var sagaresult = sagatable.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            _logger.Log($"Created {sagatable} table");

            var messagetable = GetCloudTable(_connectionstring, $"{_messagestorgename}{_tablenamesufix}");

            var messageresult = messagetable.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            _logger.Log($"Created {messagetable} table");
        }
    }
}