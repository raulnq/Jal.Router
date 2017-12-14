using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface INextListenerRouteBuilder<THandler, out TData>
    {
        INextNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
        INextNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder;
    }
}