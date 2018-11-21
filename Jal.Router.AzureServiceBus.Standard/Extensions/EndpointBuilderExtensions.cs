using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class EndpointBuilderExtensions
    {
        public static IAndWaitReplyFromEndPointBuilder AddTopic(this IToChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPublishSubscriberChannel(connectionstring, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddTopic<TExtractorConectionString>(this IToChannelBuilder builder, Func<IValueFinder, string> connectionstringextractor, string path) where TExtractorConectionString : IValueFinder
        {
            return builder.AddPublishSubscriberChannel<TExtractorConectionString>(connectionstringextractor, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue(this IToChannelBuilder builder, string connectionstring, string path)
        {
            return builder.AddPointToPointChannel(connectionstring, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddQueue<TExtractorConectionString>(this IToChannelBuilder builder, Func<IValueFinder, string> connectionstringextractor, string path) where TExtractorConectionString : IValueFinder
        {
            return builder.AddPointToPointChannel<TExtractorConectionString>(connectionstringextractor, path);
        }

        public static void AndWaitReplyFromQueue(this IAndWaitReplyFromEndPointBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromPointToPointChannel(path, connectionstring, timeout);
        }

        public static void AndWaitReplyFromQueue<TExtractorConectionString>(this IAndWaitReplyFromEndPointBuilder builder, Func<IValueFinder, string> connectionstringextractor, string path, int timeout = 60) where TExtractorConectionString : IValueFinder
        {
            builder.AndWaitReplyFromPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor, timeout);

        }

        public static void AndWaitReplyFromSubscription(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, timeout);
        }

        public static void AndWaitReplyFromSubscription<TExtractorConectionString>(this IAndWaitReplyFromEndPointBuilder builder, Func<IValueFinder, string> connectionstringextractor, string path, string subscription, int timeout = 60) where TExtractorConectionString : IValueFinder
        {
            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor, timeout);
        }
    }
}