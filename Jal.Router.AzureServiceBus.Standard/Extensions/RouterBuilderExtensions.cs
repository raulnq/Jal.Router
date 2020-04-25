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

            builder.AddResourceManager<AzureManagementPointToPointChannelResourceManager>();

            builder.AddResourceManager<AzureManagementPublishSubscribeChannelResourceManager>();

            builder.AddResourceManager<AzureManagementSubscriptionToPublishSubscribeChannelResourceManager>();

            builder.AddResourceManager<AzureServiceBusPointToPointChannelResourceManager>();

            builder.AddResourceManager<AzureServiceBusPublishSubscribeChannelResourceManager>();

            builder.AddResourceManager<AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager>();

            builder.AddMessageAdapter<AzureServiceBusMessageAdapter>();
        }
    }
}
