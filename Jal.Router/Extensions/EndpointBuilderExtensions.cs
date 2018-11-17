using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Extensions
{
    public static class EndpointBuilderExtensions
    {
        public static void To<TExtractorConnectionString>(this IToEndPointBuilder builder, Func<IValueFinder, string> connectionstringextractor, string path) where TExtractorConnectionString : IValueFinder
        {
            builder.To(x=>x.Add<TExtractorConnectionString>(connectionstringextractor, path));
        }

        public static void To(this IToEndPointBuilder builder, string connectionstring, string path)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.To<NullValueSettingFinder>(extractor, path);
        }

        public static IAndWaitReplyFromEndPointBuilder Add(this IToChannelBuilder builder, string connectionstring, string path)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.Add<NullValueSettingFinder>(extractor, path);
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.AndWaitReplyFromPointToPointChannel<NullValueSettingFinder>(path, extractor);

        }
        public static void AndWaitReplyFromPublishSubscribeChannel(this IAndWaitReplyFromEndPointBuilder builder, string path, string subscription, string connectionstring, int timeout = 60)
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.AndWaitReplyFromPublishSubscribeChannel<NullValueSettingFinder>(path, subscription, extractor);
        }

    }
}