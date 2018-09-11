using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Extensions
{
    public static class ListenerRouteBuilderExtensions
    {
        public static INameRouteBuilder<THandler> ToListenQueue<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<THandler, TExtractorConectionString>(path, connectionstringextractor);
        }

        public static INameRouteBuilder<THandler> ToListenQueue<THandler>(this IListenerRouteBuilder<THandler> listenerroutebuilder, string path, string connectionstring)
        {
            return listenerroutebuilder.ToListenPointToPointChannel(path, connectionstring);
        }

        public static INameRouteBuilder<THandler> ToListenTopic<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> listenerroutebuilder,string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<THandler, TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static INameRouteBuilder<THandler> ToListenTopic<THandler>(this IListenerRouteBuilder<THandler> listenerroutebuilder, string path, string subscription, string connectionstring)
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel(path, subscription, connectionstring);
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData, TExtractorConectionString>(this IFirstListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(path, connectionstringextractor);
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData>(this IFirstListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string connectionstring)
        {
            return listenerroutebuilder.ToListenPointToPointChannel(path, connectionstring);
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData, TExtractorConectionString>(this IFirstListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static IFirstNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData>(this IFirstListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, string connectionstring)
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel(path, subscription, connectionstring);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData, TExtractorConectionString>(this IMiddleListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(path, connectionstringextractor);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData>(this IMiddleListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string connectionstring)
        {
            return listenerroutebuilder.ToListenPointToPointChannel(path, connectionstring);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData, TExtractorConectionString>(this IMiddleListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static IMiddleNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData>(this IMiddleListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, string connectionstring)
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel(path, subscription, connectionstring);
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData, TExtractorConectionString>(this ILastListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<THandler, TData, TExtractorConectionString>(path, connectionstringextractor);
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData>(this ILastListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string connectionstring)
        {
            return listenerroutebuilder.ToListenPointToPointChannel(path, connectionstring);
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData, TExtractorConectionString>(this ILastListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<THandler, TData, TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static ILastNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData>(this ILastListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, string connectionstring)
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel(path, subscription, connectionstring);
        }
    }
}