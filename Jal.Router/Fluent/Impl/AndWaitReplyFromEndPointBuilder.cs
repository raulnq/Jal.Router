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
            _channel.UpdateReplyFromPointToPointChannel(path, timeout, typeof(TValueFinder), connectionstringprovider);
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

            _channel.UpdateReplyFromSubscriptionToPublishSubscribeChannel(path, timeout, subscription, typeof(TValueFinder), connectionstringprovider);
        }
    }
}