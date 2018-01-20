using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndingListenerRouteBuilder<THandler, out TData>
    {
        IEndingNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
        IEndingNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
    }
}