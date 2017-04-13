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
    }
}