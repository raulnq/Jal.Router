using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class AndWaitReplyFromBuilder : IAndWaitReplyFromBuilder
    {
        private readonly Channel _channel;
        public AndWaitReplyFromBuilder(Channel channel)
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

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path, null, null);

            _channel.ReplyTo(channel, timeout);
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

            var channel = new Channel(ChannelType.SubscriptionToPublishSubscribe, connectionstring, path, null, null);

            _channel.ReplyTo(channel, timeout);
        }
    }
}