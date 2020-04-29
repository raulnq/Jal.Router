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

        public void AndWaitReplyFromPointToPointChannel(string path, string connectionstring, int timeout = 60, Type adapter = null, Type type = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }
            if (type != null && !typeof(IRequestReplyChannelFromPointToPointChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }


            _channel.UpdateReplyFromPointToPointChannel(path, timeout, connectionstring, adapter, type);
        }

        public void AndWaitReplyFromSubscriptionToPublishSubscribeChannel(string path, string subscription,
            string connectionstring, int timeout = 60, Type adapter = null, Type type = null)
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
            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }
            if (type != null && !typeof(IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }


            _channel.UpdateReplyFromSubscriptionToPublishSubscribeChannel(path, timeout, subscription, connectionstring, adapter, type);
        }
    }
}