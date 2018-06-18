using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class AndWaitReplyFromEndPointBuilder : IAndWaitReplyFromEndPointBuilder
    {
        private readonly Channel _channel;
        public AndWaitReplyFromEndPointBuilder(Channel channel)
        {
            _channel = channel;
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
            _channel.ToReplyPath = path;

            _channel.ToReplyConnectionStringExtractor = connectionstringextractor;

            _channel.ReplyConnectionStringExtractorType = typeof(TExtractorConectionString);

            _channel.ToReplyTimeOut = timeout;
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

            _channel.ToReplyPath = path;

            _channel.ToReplyConnectionStringExtractor = connectionstringextractor;

            _channel.ReplyConnectionStringExtractorType = typeof(TExtractorConectionString);

            _channel.ToReplySubscription = subscription;

            _channel.ToReplyTimeOut = timeout;
        }
    }
}