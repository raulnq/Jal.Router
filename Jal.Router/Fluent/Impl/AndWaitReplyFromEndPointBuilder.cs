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

        public void AndWaitReplyFromPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider, int timeout = 60) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            _channel.ToReplyPath = path;

            _channel.ToReplyConnectionStringProvider = connectionstringprovider;

            _channel.ReplyConnectionStringValueFinderType = typeof(TValueFinder);

            _channel.Type = ChannelType.RequestReplyToPointToPoint;

            _channel.ToReplyTimeOut = timeout;
        }

        public void AndWaitReplyFromSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription,
            Func<IValueFinder, string> connectionstringprovider, int timeout = 60) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _channel.ToReplyPath = path;

            _channel.ToReplyConnectionStringProvider = connectionstringprovider;

            _channel.ReplyConnectionStringValueFinderType = typeof(TValueFinder);

            _channel.ToReplySubscription = subscription;

            _channel.Type = ChannelType.RequestReplyToSubscriptionToPublishSubscribe;

            _channel.ToReplyTimeOut = timeout;
        }
    }
}