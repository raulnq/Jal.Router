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
        public override string Send(MessageContext context, object message)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var bm = message as BrokeredMessage;

            if (bm != null)
            {
                topicClient.Send(bm);

                topicClient.Close();

                return bm.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(Route route, Action<object>[] routeactions, string channelpath)
        {
            var client = SubscriptionClient.CreateFromConnectionString(route.ToConnectionString, route.ToPath, route.ToSubscription);

            var path = SubscriptionClient.FormatSubscriptionPath(route.ToPath, route.ToSubscription);

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

            route.ShutdownAction = () => { receiver.Close(); client.Close(); };
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

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null)
            : base(factory, configuration, builder)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}