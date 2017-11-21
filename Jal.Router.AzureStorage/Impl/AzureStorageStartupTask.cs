using Jal.Router.Interface.Management;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureStorageStartupTask : IStartupTask
    {
        private readonly string _connectionstring;

        private readonly string _sagastoragename;

        private readonly string _messagestorgename;

        private readonly string _tablenamesufix;

        public AzureStorageStartupTask(string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            _connectionstring = connectionstring;

            _tablenamesufix = tablenamesufix;

            _sagastoragename = sagastoragename;

            _messagestorgename = messagestorgename;
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

            sagatable.CreateIfNotExists();

            var messagetable = GetCloudTable(_connectionstring, $"{_messagestorgename}{_tablenamesufix}");

            messagetable.CreateIfNotExists();
        }
    }
}