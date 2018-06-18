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

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            builder.AddPointToPointChannel<NullValueSettingFinder>(path, extractor);
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

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            builder.AddPublishSubscribeChannel<NullValueSettingFinder>(path, subscription, extractor);
        }

        public static INameRouteBuilder<THandler> ToListenPointToPointChannel<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> builder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static INameRouteBuilder<THandler> ToListenPublishSubscribeChannel<THandler,TExtractorConectionString>(this IListenerRouteBuilder<THandler> builder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static INameRouteBuilder<THandler> ToListenPointToPointChannel<THandler>(this IListenerRouteBuilder<THandler> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, NullValueSettingFinder>(path, extractor);
        }

        public static INameRouteBuilder<THandler> ToListenPublishSubscribeChannel<THandler>(this IListenerRouteBuilder<THandler> builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, NullValueSettingFinder>(path, subscription, extractor);
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this INextListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this INextListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this INextListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this INextListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
        public static IStartingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this IStartingListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static IStartingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this IStartingListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static IStartingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this IStartingListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static IStartingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this IStartingListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
        public static IEndingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this IEndingListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static IEndingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this IEndingListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static IEndingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this IEndingListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static IEndingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this IEndingListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
    }
}