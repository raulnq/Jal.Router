using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IAndWaitReplyFromEndPointBuilder
    {
        void AndWaitReplyFromPointToPointChannel<TExtractorConectionString>(string path, Func<IValueFinder, string> connectionstringextractor, int timeout = 60)
            where TExtractorConectionString : IValueFinder;
        void AndWaitReplyFromPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueFinder, string> connectionstringextractor, int timeout = 60)
            where TExtractorConectionString : IValueFinder;
    }
}