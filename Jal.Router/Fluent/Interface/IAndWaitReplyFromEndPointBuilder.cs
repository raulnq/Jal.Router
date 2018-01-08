using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IAndWaitReplyFromEndPointBuilder
    {
        void AndWaitReplyFromPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor, int timeout = 60)
            where TExtractorConectionString : IValueSettingFinder;
        void AndWaitReplyFromPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor, int timeout = 60)
            where TExtractorConectionString : IValueSettingFinder;
    }
}