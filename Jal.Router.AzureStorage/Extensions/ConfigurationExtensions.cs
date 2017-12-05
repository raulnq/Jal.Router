using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureStorage.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingAzureStorage(this IConfiguration configuration)
        {
            configuration.UsingStorage<AzureTableStorage>();

            configuration.AddStartupTask<AzureStorageStartupTask>();
        }
    }
}