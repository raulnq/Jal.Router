using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : AbstractChannel, IPublishSubscribeChannel
    {
        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { TopicClient.CreateFromConnectionString(metadata.ToConnectionString, metadata.ToPath) };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return sender =>
            {
                var client = sender[0] as TopicClient;

                client.Close();
            };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (sender, message) =>
            {
                var client = sender[0] as TopicClient;

                var bm = message as BrokeredMessage;

                if (bm != null)
                {
                    client.Send(bm);

                    return bm.MessageId;
                }

                return string.Empty;
            };
        }

        public Func<object[]> CreateListenerMethodFactory(ListenerMetadata metadata)
        {
            return () => 
            {
                var client = SubscriptionClient.CreateFromConnectionString(metadata.ToConnectionString, metadata.ToPath, metadata.ToSubscription);

                var channelpath = metadata.GetPath();

                var path = SubscriptionClient.FormatSubscriptionPath(metadata.ToPath, metadata.ToSubscription);

                var receiver = client.MessagingFactory.CreateMessageReceiver(path);

                return new object[] { client, receiver };
            };
        }

        public Action<object[]> DestroyListenerMethodFactory(ListenerMetadata metadata)
        {
            return listener =>
            {
                var client = listener[0] as SubscriptionClient;

                var receiver = listener[1] as MessageReceiver;

                receiver.Close();

                client.Close();
            };
        }

        public Action<object[]> ListenerMethodFactory(ListenerMetadata metadata)
        {
            var options = CreateOptions();

            Action<BrokeredMessage> handler = message =>
            {
                foreach (var action in metadata.Handlers)
                {
                    var clone = message.Clone();

                    action(clone);
                }
            };

            var channelpath = metadata.GetPath();

            return (listener) =>
            {
                var client = listener[0] as SubscriptionClient;

                var receiver = listener[1] as MessageReceiver;

                receiver.OnMessage(bm => OnMessage(metadata, bm.MessageId, () => handler(bm), () => client.Complete(bm.LockToken)), options);
            };
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