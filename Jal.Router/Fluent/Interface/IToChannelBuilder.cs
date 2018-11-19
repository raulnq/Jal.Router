using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        IAndWaitReplyFromEndPointBuilder AddPointToPointChannel<TExtractorConnectionString>(Func<IValueFinder, string> connectionstringextractor, string path)
    where TExtractorConnectionString : IValueFinder;

        IAndWaitReplyFromEndPointBuilder AddPublishSubscriberChannel<TExtractorConnectionString>(Func<IValueFinder, string> connectionstringextractor, string path)
    where TExtractorConnectionString : IValueFinder;
    }
}