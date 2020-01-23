using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{

    public static class EndpointBuilderExtensions
    {
        public static void AddTopic(this IToChannelBuilder builder, string connectionstring, string path)
        {
            builder.AddPublishSubscribeChannel(connectionstring, path);
        }

        public static void AddQueue(this IToChannelBuilder builder, string connectionstring, string path)
        {
            builder.AddPointToPointChannel(connectionstring, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue(this IToReplyChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path);
        }

        public static void AndWaitReplyFromQueue(this IAndWaitReplyFromEndPointBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromPointToPointChannel(path, connectionstring, timeout);
        }

        public static void AndWaitReplyFromSubscriptionToTopic(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, timeout);
        }
    }
}