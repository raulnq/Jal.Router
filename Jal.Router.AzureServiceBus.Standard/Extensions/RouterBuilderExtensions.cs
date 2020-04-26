using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class RouterBuilderExtensions
    {
        public static void AddAzureServiceBus(this IRouterBuilder builder)
        {
            builder.AddPointToPointChannel<AzureServiceBusQueue>();

            builder.AddPublishSubscribeChannel<AzureServiceBusTopic>();

            builder.AddRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>();

            builder.AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>();

            builder.AddResourceManager<AzureManagementPointToPointResourceManager>();

            builder.AddResourceManager<AzureManagementPublishSubscribeResourceManager>();

            builder.AddResourceManager<AzureManagementSubscriptionToPublishSubscribeResourceManager>();

            builder.AddResourceManager<AzureServiceBusPointToPointResourceManager>();

            builder.AddResourceManager<AzureServiceBusPublishSubscribeResourceManager>();

            builder.AddResourceManager<AzureServiceBusSubscriptionToPublishSubscribeResourceManager>();

            builder.AddMessageAdapter<AzureServiceBusMessageAdapter>();
        }
    }
}
