using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToReplyChannelBuilder
    {
        IAndWaitReplyFromEndPointBuilder AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path)
    where TValueFinder : IValueFinder;

    }
}