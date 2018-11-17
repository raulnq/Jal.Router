using System;
using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override string Send(Channel channel, object message)
        {
            var queueclient = new QueueClient(channel.ToConnectionString, channel.ToPath);

            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                queueclient.SendAsync(sbmessage).GetAwaiter().GetResult();

                queueclient.CloseAsync().GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(ListenerMetadata metadata)
        {
            var queueclient = new QueueClient(metadata.ToConnectionString, metadata.ToPath);

            var path = metadata.GetPath();

            var options = CreateOptions(path);

            Action<Message> @action = message =>
            {
                foreach (var routingaction in metadata.Handlers)
                {
                    var clone = message.Clone();

                    routingaction(clone);
                }
            };

            queueclient.RegisterMessageHandler(async (message, token) =>
            {
                await OnMessageAsync(path, message.MessageId, () => @action(message), () => queueclient.CompleteAsync(message.SystemProperties.LockToken));
            }, options);

            metadata.Shutdown = () => { queueclient.CloseAsync().GetAwaiter().GetResult(); };
        }

        private MessageHandlerOptions CreateOptions(string channelpath)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                Logger.Log($"Message failed to {Name} channel {channelpath} {args.Exception}");
                return Task.CompletedTask;
            } ;

            var options = new MessageHandlerOptions(handler) {AutoComplete = false};

            if (_maxconcurrentcalls > 0)
            {
                options.MaxConcurrentCalls = _maxconcurrentcalls;
            }
            if (_autorenewtimeout != null)
            {
                options.MaxAutoRenewDuration = _autorenewtimeout.Value;
            }
            return options;
        }

        private readonly int _maxconcurrentcalls;

        private readonly TimeSpan? _autorenewtimeout;

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration, ILogger logger, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null) 
            : base(factory, configuration, logger)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}