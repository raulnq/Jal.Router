using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class EndpointChannelBuilderExtensions
    {
        public static IChannelIWhenBuilder AddTopic(this IEndpointChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPublishSubscribeChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }

        public static IChannelIWhenBuilder AddQueue(this IEndpointChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue));
        }

        public static IAndWaitReplyFromBuilder AddQueue(this IReplyIEndpointChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path);
        }

        public static void AndWaitReplyFromQueue(this IAndWaitReplyFromBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromPointToPointChannel(path, connectionstring, timeout);
        }

        public static void AndWaitReplyFromSubscriptionToTopic(this IAndWaitReplyFromBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, timeout);
        }
    }
}