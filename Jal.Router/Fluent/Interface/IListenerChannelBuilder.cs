using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        void AddPointToPointChannel<TExtractorConectionString>(string path, Func<IValueFinder, string> connectionstringextractor) where TExtractorConectionString : IValueFinder;
        void AddPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription, Func<IValueFinder, string> connectionstringextractor) where TExtractorConectionString : IValueFinder;
    }
}