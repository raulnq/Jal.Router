using Jal.Router.AzureStorage.Impl;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureStorage.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseAzureStorage(this IConfiguration configuration, AzureStorageParameter parameter)
        {
            return configuration
                .UseStorage<Impl.AzureEntityStorage>()
                .AddStartupTask<AzureEntityStorageStartupTask>()
                .UseMessageStorage<AzureMessageStorage>()
                .AddStartupTask<AzureMessageStorageStartupTask>()
                .AddParameter(parameter);
        }
    }
}