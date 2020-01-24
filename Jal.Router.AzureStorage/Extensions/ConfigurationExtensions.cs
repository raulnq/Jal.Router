using Jal.Router.AzureStorage.Impl;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureStorage.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseAzureStorageAsStorage(this IConfiguration configuration, AzureStorageParameter parameter)
        {
            return configuration
                .UseEntityStorage<AzureEntityStorage>()
                .AddStartupTask<AzureEntityStorageStartupTask>()
                .UseMessageStorage<AzureMessageStorage>()
                .AddStartupTask<AzureMessageStorageStartupTask>()
                .AddParameter(parameter);
        }
    }
}