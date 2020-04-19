using Jal.Router.Interface;

namespace Jal.Router.AzureStorage
{
    public static class RouterBuilderExtensions
    {
        public static void AddAzureStorage(this IRouterBuilder builder)
        {
            builder.AddMessageStorage<AzureMessageStorage>();

            builder.AddEntityStorage<AzureEntityStorage>();

            builder.AddStartupTask<AzureMessageStorageStartupTask>();

            builder.AddStartupTask<AzureEntityStorageStartupTask>();
        }
    }
}
