using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class ListenerChannelBuilderExtensions
    {
        public static void AddQueue(this IListenerChannelBuilder builder, string path, string connectionstring)
        {
            builder.AddPointToPointChannel(path, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue) );
        }
        public static void AddSubscriptionToTopic(this IListenerChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            builder.AddSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }
    }
}