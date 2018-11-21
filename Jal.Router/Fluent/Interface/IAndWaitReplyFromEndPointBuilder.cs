using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IAndWaitReplyFromEndPointBuilder
    {
        void AndWaitReplyFromPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider, int timeout = 60)
            where TValueFinder : IValueFinder;
        void AndWaitReplyFromSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider, int timeout = 60)
            where TValueFinder : IValueFinder;
    }
}