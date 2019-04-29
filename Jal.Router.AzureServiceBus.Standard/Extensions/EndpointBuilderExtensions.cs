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

        public static void AddTopic<TValueFinder>(this IToChannelBuilder builder, Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            builder.AddPublishSubscribeChannel<TValueFinder>(connectionstringprovider, path);
        }

        public static void AddQueue(this IToChannelBuilder builder, string connectionstring, string path)
        {
            builder.AddPointToPointChannel(connectionstring, path);
        }

        public static void AddQueue<TValueFinder>(this IToChannelBuilder builder, Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            builder.AddPointToPointChannel<TValueFinder>(connectionstringprovider, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue(this IToReplyChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue<TValueFinder>(this IToReplyChannelBuilder builder, Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            return builder.AddPointToPointChannel<TValueFinder>(connectionstringprovider, path);
        }

        public static void AndWaitReplyFromQueue(this IAndWaitReplyFromEndPointBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromPointToPointChannel(path, connectionstring, timeout);
        }

        public static void AndWaitReplyFromQueue<TValueFinder>(this IAndWaitReplyFromEndPointBuilder builder, Func<IValueFinder, string> connectionstringprovider, string path, int timeout = 60) where TValueFinder : IValueFinder
        {
            builder.AndWaitReplyFromPointToPointChannel<TValueFinder>(path, connectionstringprovider, timeout);

        }

        public static void AndWaitReplyFromSubscriptionToTopic<TReplayType>(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel<TReplayType>(path, subscription, connectionstring, timeout);
        }

        public static void AndWaitReplyFromSubscriptionToTopic<TValueFinder>(this IAndWaitReplyFromEndPointBuilder builder, Func<IValueFinder, string> connectionstringprovider, string path, string subscription, int timeout = 60) where TValueFinder : IValueFinder
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel<TValueFinder>(path, subscription, connectionstringprovider, timeout);
        }
    }
}