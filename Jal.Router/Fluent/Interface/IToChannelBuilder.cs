using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        IAndWaitReplyFromEndPointBuilder AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path)
    where TValueFinder : IValueFinder;

        IAndWaitReplyFromEndPointBuilder AddPublishSubscriberChannel<TValue>(Func<IValueFinder, string> connectionstringprovider, string path)
    where TValue : IValueFinder;
    }
}