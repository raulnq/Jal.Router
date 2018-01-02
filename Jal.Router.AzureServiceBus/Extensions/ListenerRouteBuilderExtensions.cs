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
    }
}