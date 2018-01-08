using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        IAndWaitReplyFromEndPointBuilder To<TExtractorConnectionString>(Func<IValueSettingFinder, string> connectionstringextractor, string path)
            where TExtractorConnectionString : IValueSettingFinder;
    }
}