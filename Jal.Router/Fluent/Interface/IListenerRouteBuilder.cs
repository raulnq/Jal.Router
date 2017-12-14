using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerRouteBuilder<THandler>
    {
        INameRouteBuilder<THandler> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
        INameRouteBuilder<THandler> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
    }
}