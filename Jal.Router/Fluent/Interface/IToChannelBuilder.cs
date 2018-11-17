using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        IAndWaitReplyFromEndPointBuilder Add<TExtractorConnectionString>(Func<IValueFinder, string> connectionstringextractor, string path)
    where TExtractorConnectionString : IValueFinder;
    }
}