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

            Func<IValueFinder, string> extractor = x => connectionstring;

            builder.AddPublishSubscribeChannel<NullValueSettingFinder>(path, subscription, extractor);
        }

        public static INameRouteBuilder<THandler> ToListenPointToPointChannel<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> builder, string path, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static INameRouteBuilder<THandler> ToListenPublishSubscribeChannel<THandler,TExtractorConectionString>(this IListenerRouteBuilder<THandler> builder, string path, string subscription, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
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

            Func<IValueFinder, string> extractor = x => connectionstring;

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

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, NullValueSettingFinder>(path, subscription, extractor);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this IMiddleListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueFinder, string> connectionstringextractor)
where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this IMiddleListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this IMiddleListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this IMiddleListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
        public static IFirstNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this IFirstListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this IFirstListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this IFirstListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this IFirstListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
        public static ILastNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(this ILastListenerRouteBuilder<THandler, TData> builder, string path, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor));
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(this ILastListenerRouteBuilder<THandler, TData> builder, string path, string subscription, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            return builder.ToListen(x => x.AddPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor));
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<THandler, TData>(this ILastListenerRouteBuilder<THandler, TData> builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPointToPointChannel<THandler, TData, NullValueSettingFinder>(path, extractor);
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<THandler, TData>(this ILastListenerRouteBuilder<THandler, TData> builder, string path, string subscription, string connectionstring)
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            return builder.ToListenPublishSubscribeChannel<THandler, TData, NullValueSettingFinder>(path, subscription, extractor);
        }
    }
}