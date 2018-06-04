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

        void AndWaitReplyFromPointToPointChannel(string path, string connectionstring, int timeout = 60);

        void AndWaitReplyFromPublishSubscribeChannel(string path, string subscription, string connectionstring, int timeout = 60);
    }
}