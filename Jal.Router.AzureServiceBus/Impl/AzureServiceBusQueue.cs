using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override string Send(Channel channel, object message)
        {
            var queueclient = QueueClient.CreateFromConnectionString(channel.ToConnectionString, channel.ToPath);

            var bm = message as BrokeredMessage;

            if (bm != null)
            {
                queueclient.Send(bm);

                queueclient.Close();

                return bm.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(ListenerMetadata metadata)
        {
            var client = QueueClient.CreateFromConnectionString(metadata.ToConnectionString, metadata.ToPath);
            
            var receiver = client.MessagingFactory.CreateMessageReceiver(metadata.ToPath);

            var channelpath = metadata.GetPath();

            var options = CreateOptions();

            Action<BrokeredMessage> routeaction = message =>
            {
                foreach (var action in metadata.Handlers)
                {
                    var clone = message.Clone();
                    action(clone);
                }
            };


            receiver.OnMessage(bm => OnMessage(channelpath, bm.MessageId,()=> routeaction(bm), () => client.Complete(bm.LockToken)), options);

            metadata.Shutdown = () => { receiver.Close(); client.Close(); };
        }

        private OnMessageOptions CreateOptions()
        {
            var options = new OnMessageOptions() {AutoComplete = false};

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

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration, ILogger logger,  int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null) 
            : base(factory, configuration, logger)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}