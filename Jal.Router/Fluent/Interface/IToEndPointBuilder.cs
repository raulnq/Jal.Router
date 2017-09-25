using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        void To<TExtractorConnectionString, TExtractorPath>(Func<IValueSettingFinder, string> connectionstringextractor, Func<IValueSettingFinder, string> pathextractor) 
            where TExtractorConnectionString : IValueSettingFinder
            where TExtractorPath : IValueSettingFinder
            ;

        void To<TExtractorConnectionString>(Func<IValueSettingFinder, string> connectionstringextractor, string path)
    where TExtractorConnectionString : IValueSettingFinder
    ;
    }
}