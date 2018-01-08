using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Extensions
{
    public static class ListenerRouteBuilderExtensions
    {
        public static INameRouteBuilder<THandler> ToListenQueue<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor);
        }

        public static INameRouteBuilder<THandler> ToListenTopic<THandler, TExtractorConectionString>(this IListenerRouteBuilder<THandler> listenerroutebuilder,string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static IStartingNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData, TExtractorConectionString>(this IStartingListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor);
        }

        public static IStartingNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData, TExtractorConectionString>(this IStartingListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor);
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenQueue<THandler, TData, TExtractorConectionString>(this INextListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, Func<IValueSettingFinder, string> connectionstringextractor)
where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPointToPointChannel<TExtractorConectionString>(path, connectionstringextractor);
        }

        public static INextNameRouteBuilder<THandler, TData> ToListenTopic<THandler, TData, TExtractorConectionString>(this INextListenerRouteBuilder<THandler, TData> listenerroutebuilder, string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
    where TExtractorConectionString : IValueSettingFinder
        {
            return listenerroutebuilder.ToListenPublishSubscribeChannel<TExtractorConectionString>(path, subscription, connectionstringextractor);
        }
    }
}