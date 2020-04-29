using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class ListenerChannelBuilderExtensions
    {
        public static IChannelIWhenBuilder AddQueue(this IListenerChannelBuilder builder, string path, string connectionstring)
        {
            return builder.AddPointToPointChannel(path, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue) );
        }
        public static IChannelIWhenBuilder AddSubscriptionToTopic(this IListenerChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            return builder.AddSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }
    }
}