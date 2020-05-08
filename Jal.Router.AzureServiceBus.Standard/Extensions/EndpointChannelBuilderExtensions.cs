using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class EndpointChannelBuilderExtensions
    {
        public static IChannelIWhenBuilder AddTopic(this IEndpointChannelBuilder builder, string connectionstring, string path, AzureServiceBusChannelConnection connection = null)
        {
            return builder.AddPublishSubscribeChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic), connection?.ToDictionary());
        }

        public static IChannelIWhenBuilder AddQueue(this IEndpointChannelBuilder builder, string connectionstring, string path, AzureServiceBusChannelConnection connection = null)
        {
            return builder.AddPointToPointChannel(connectionstring, path, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue), connection?.ToDictionary());
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