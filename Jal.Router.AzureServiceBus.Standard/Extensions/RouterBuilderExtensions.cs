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

            builder.AddChannelResourceManager<AzureManagementPointToPointChannelResourceManager, PointToPointChannelResource, PointToPointChannelStatistics>();

            builder.AddChannelResourceManager<AzureManagementPublishSubscribeChannelResourceManager, PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<AzureManagementSubscriptionToPublishSubscribeChannelResourceManager, SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<AzureServiceBusPointToPointChannelResourceManager, PointToPointChannelResource, PointToPointChannelStatistics>();

            builder.AddChannelResourceManager<AzureServiceBusPublishSubscribeChannelResourceManager, PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager, SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddMessageAdapter<AzureServiceBusMessageAdapter>();
        }
    }
}
