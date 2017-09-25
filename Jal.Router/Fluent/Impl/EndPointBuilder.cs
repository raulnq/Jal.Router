using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IToEndPointBuilder, INameEndPointBuilder
    {
        private readonly EndPoint _endpoint;

        public EndPointBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public IToEndPointBuilder ForMessage<TMessage>()
        {
            _endpoint.MessageType = typeof (TMessage);

            return this;
        }


        public void To<TExtractorConnectionString, TExtractorPath>(Func<IValueSettingFinder, string> connectionstringextractor, Func<IValueSettingFinder, string> pathextractor) 
            where TExtractorConnectionString : IValueSettingFinder
            where TExtractorPath : IValueSettingFinder
        {
            _endpoint.ConnectionStringExtractorType = typeof(TExtractorConnectionString);

            _endpoint.PathExtractorType = typeof(TExtractorPath);

            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            if (pathextractor == null)
            {
                throw new ArgumentNullException(nameof(pathextractor));
            }

            _endpoint.ToConnectionStringExtractor = connectionstringextractor;

            _endpoint.ToPathExtractor = pathextractor;
        }

        public void To<TExtractorConnectionString>(Func<IValueSettingFinder, string> connectionstringextractor, string path) where TExtractorConnectionString : IValueSettingFinder
        {
            _endpoint.ConnectionStringExtractorType = typeof(TExtractorConnectionString);

            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _endpoint.ToConnectionStringExtractor = connectionstringextractor;

            _endpoint.ToPath = path;
        }
    }
}