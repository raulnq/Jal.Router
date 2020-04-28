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

            builder.AddResource<AzureManagementPointToPointResource>();

            builder.AddResource<AzureManagementPublishSubscribeResource>();

            builder.AddResource<AzureManagementSubscriptionToPublishSubscribeResource>();

            builder.AddResource<AzureServiceBusPointToPointResource>();

            builder.AddResource<AzureServiceBusPublishSubscribeResource>();

            builder.AddResource<AzureServiceBusSubscriptionToPublishSubscribeResource>();

            builder.AddMessageAdapter<AzureServiceBusMessageAdapter>();
        }
    }
}
