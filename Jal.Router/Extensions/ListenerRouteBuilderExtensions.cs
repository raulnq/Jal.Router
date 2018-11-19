using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Extensions
{
    public static class ListenerRouteBuilderExtensions
    {
        public static void AddPointToPointChannel(this IListenerChannelBuilder builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstring == null)
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.AddPointToPointChannel<NullValueFinder>(path, extractor);
        }

        public static void AddPublishSubscribeChannel(this IListenerChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstring == null)
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.AddPublishSubscribeChannel<NullValueFinder>(path, subscription, extractor);
        }
    }
}