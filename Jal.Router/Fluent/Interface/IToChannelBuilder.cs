using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        void AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path)
    where TValueFinder : IValueFinder;

        void AddPublishSubscribeChannel<TValue>(Func<IValueFinder, string> connectionstringprovider, string path)
    where TValue : IValueFinder;
    }
}