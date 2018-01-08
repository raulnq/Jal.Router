using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IToEndPointBuilder, INameEndPointBuilder, IAndWaitReplyFromEndPointBuilder
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

        public IAndWaitReplyFromEndPointBuilder To<TExtractorConnectionString>(Func<IValueSettingFinder, string> connectionstringextractor, string path) where TExtractorConnectionString : IValueSettingFinder
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

            return this;
        }

        public void AndWaitReplyFromPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor, int timeout = 60) where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            _endpoint.ToReplyPath = path;

            _endpoint.ToReplyConnectionStringExtractor = connectionstringextractor;

            _endpoint.ReplyConnectionStringExtractorType = typeof(TExtractorConectionString);

            _endpoint.ToReplyTimeOut = timeout;
        }

        public void AndWaitReplyFromPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription,
            Func<IValueSettingFinder, string> connectionstringextractor, int timeout = 60) where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _endpoint.ToReplyPath = path;

            _endpoint.ToReplyConnectionStringExtractor = connectionstringextractor;

            _endpoint.ReplyConnectionStringExtractorType = typeof(TExtractorConectionString);

            _endpoint.ToReplySubscription = subscription;

            _endpoint.ToReplyTimeOut = timeout;
        }
    }
}