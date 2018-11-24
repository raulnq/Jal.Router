using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl.ValueFinder;
using Jal.Router.Interface;

namespace Jal.Router.Extensions
{
    public static class EndpointBuilderExtensions
    {
        public static IAndWaitReplyFromEndPointBuilder AddPointToPointChannel(this IToChannelBuilder builder, string connectionstring, string path)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            return builder.AddPointToPointChannel<NullValueFinder>(provider, path);
        }

        public static IAndWaitReplyFromEndPointBuilder AddPublishSubscriberChannel(this IToChannelBuilder builder, string connectionstring, string path)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            return builder.AddPublishSubscriberChannel<NullValueFinder>(provider, path);
        }

        public static void AndWaitReplyFromPointToPointChannel(this IAndWaitReplyFromEndPointBuilder builder, string path, string connectionstring, int timeout = 60)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            builder.AndWaitReplyFromPointToPointChannel<NullValueFinder>(path, provider, timeout);

        }
        public static void AndWaitReplyFromSubscriptionToPublishSubscribeChannel(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            builder.AndWaitReplyFromSubscriptionToPublishSubscribeChannel<NullValueFinder>(path, subscription, provider, timeout);
        }

    }
}