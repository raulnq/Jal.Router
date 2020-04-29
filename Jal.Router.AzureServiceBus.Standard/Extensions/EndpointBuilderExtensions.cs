using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class EndpointBuilderExtensions
    {
        public static void AddTopic(this IToChannelBuilder builder, string connectionstring, string path)
        {
            builder.AddPublishSubscribeChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }

        public static void AddQueue(this IToChannelBuilder builder, string connectionstring, string path)
        {
            builder.AddPointToPointChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue));
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue(this IToReplyChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path);
        }

        public static void AndWaitReplyFromQueue(this IAndWaitReplyFromEndPointBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromPointToPointChannel(path, connectionstring, timeout, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusRequestReplyFromPointToPointChannel));
        }

        public static void AndWaitReplyFromSubscriptionToTopic(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, timeout, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel));
        }
    }
}