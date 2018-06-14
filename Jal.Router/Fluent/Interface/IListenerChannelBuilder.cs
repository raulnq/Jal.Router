using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        void Add(string path, string connectionstring);
        void Add(string path, string subscription, string connectionstring);
        void Add<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder;
        void Add<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder;
    }
}