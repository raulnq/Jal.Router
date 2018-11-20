using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        void AddPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
        void AddPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
    }
}