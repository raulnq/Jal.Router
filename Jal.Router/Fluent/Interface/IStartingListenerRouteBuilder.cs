using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IStartingListenerRouteBuilder<THandler, out TData>
    {
        IStartingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
        IStartingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;

        IStartingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel(string path, string connectionstring);
        IStartingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel(string path, string subscription, string connectionstring);
    }
}