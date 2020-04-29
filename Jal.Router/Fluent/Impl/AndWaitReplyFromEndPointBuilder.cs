using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class AndWaitReplyFromEndPointBuilder : IAndWaitReplyFromBuilder
    {
        private readonly Channel _channel;
        public AndWaitReplyFromEndPointBuilder(Channel channel)
        {
            _channel = channel;
        }

        public void AndWaitReplyFromPointToPointChannel(string path, string connectionstring, int timeout = 60)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            _channel.ReplyTo(ReplyType.FromPointToPoint, path, timeout, connectionstring);
        }

        public void AndWaitReplyFromSubscriptionToPublishSubscribeChannel(string path, string subscription,
            string connectionstring, int timeout = 60)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _channel.ReplyTo(ReplyType.FromSubscriptionToPublishSubscribe, path, timeout, connectionstring, subscription);
        }
    }
}