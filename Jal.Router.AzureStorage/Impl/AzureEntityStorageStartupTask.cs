using Jal.Router.AzureStorage.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Jal.Router.AzureStorage.Impl
{
    public class AzureEntityStorageStartupTask : IStartupTask
    {
        private readonly ILogger _logger;

        private readonly AzureStorageParameter _parameter;

        public AzureEntityStorageStartupTask(ILogger logger, IParameterProvider provider)
        {
            _parameter = provider.Get<AzureStorageParameter>();

            _logger = logger;
        }

        private static CloudTable GetCloudTable(string connectionstring, string tablename)
        {
            var account = CloudStorageAccount.Parse(connectionstring);

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tablename);

            return table;
        }

        public async Task Run()
        {

            if(!string.IsNullOrWhiteSpace(_parameter.TableStorageConnectionString))
            {
                var sagatable = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.SagaTableName}{_parameter.TableSufix}");

                var sagaresult = await sagatable.CreateIfNotExistsAsync().ConfigureAwait(false);

                if (sagaresult)
                {
                    _logger.Log($"Created {sagatable} table");
                }
                else
                {
                    _logger.Log($"Table {sagatable} already exists");
                }

                var messagetable = GetCloudTable(_parameter.TableStorageConnectionString, $"{_parameter.MessageTableName}{_parameter.TableSufix}");

                var messageresult =await messagetable.CreateIfNotExistsAsync().ConfigureAwait(false);

                if (messageresult)
                {
                    _logger.Log($"Created {messagetable} table");
                }
                else
                {
                    _logger.Log($"Table {messagetable} already exists");
                }
            }
            else
            {
                _logger.Log($"Skipped creation of table for sagas and messages");
            }
        }
    }
}