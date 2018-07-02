using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : AbstractPublishSubscribeChannel
    {
        public override string Send(Channel channel, object message)
        {
            var topicClient = TopicClient.CreateFromConnectionString(channel.ToConnectionString, channel.ToPath);

            var bm = message as BrokeredMessage;

            if (bm != null)
            {
                topicClient.Send(bm);

                topicClient.Close();

                return bm.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(Channel channel, Action<object>[] routeactions, string channelpath)
        {
            var client = SubscriptionClient.CreateFromConnectionString(channel.ToConnectionString, channel.ToPath, channel.ToSubscription);

            var path = SubscriptionClient.FormatSubscriptionPath(channel.ToPath, channel.ToSubscription);

            var receiver = client.MessagingFactory.CreateMessageReceiver(path);

            var options = CreateOptions();

            Action<BrokeredMessage> routeaction = message =>
            {
                foreach (var action in routeactions)
                {
                    var clone = message.Clone();
                    action(clone);
                }
            };

            receiver.OnMessage(bm => OnMessage(channelpath, bm.MessageId, () => routeaction(bm), () => client.Complete(bm.LockToken)), options);

            channel.Shutdown = () => { receiver.Close(); client.Close(); };
        }

        private OnMessageOptions CreateOptions()
        {
            var options = new OnMessageOptions() { AutoComplete = false };

            if (_maxconcurrentcalls > 0)
            {
                options.MaxConcurrentCalls = _maxconcurrentcalls;
            }
            if (_autorenewtimeout != null)
            {
                options.AutoRenewTimeout = _autorenewtimeout.Value;
            }
            return options;
        }

        private readonly int _maxconcurrentcalls;

        private readonly TimeSpan? _autorenewtimeout;

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration, ILogger logger, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null)
            : base(factory, configuration, logger)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}