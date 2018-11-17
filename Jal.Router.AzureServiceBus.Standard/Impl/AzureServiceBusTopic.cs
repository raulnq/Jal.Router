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
    public class AzureServiceBusTopic : AbstractPublishSubscribeChannel
    {
        public override string Send(Channel channel, object message)
        {
            var topicclient = new TopicClient(channel.ToConnectionString, channel.ToPath);

            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                topicclient.SendAsync(sbmessage).GetAwaiter().GetResult();

                topicclient.CloseAsync().GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(ListenerMetadata metadata)
        {
            var client = new SubscriptionClient(metadata.ToConnectionString, metadata.ToPath, metadata.ToSubscription);

            var channelpath = metadata.GetPath();

            var options = CreateOptions(channelpath);

            Action<Message> @action = message =>
            {
                foreach (var routingaction in metadata.Handlers)
                {
                    var clone = message.Clone();

                    routingaction(clone);
                }
            };


            client.RegisterMessageHandler(async (message, token) =>
            {
                await OnMessageAsync(channelpath, message.MessageId, () => @action(message), () => client.CompleteAsync(message.SystemProperties.LockToken));
            }, options);

            metadata.Shutdown = () => { client.CloseAsync().GetAwaiter().GetResult(); };
        }

        private MessageHandlerOptions CreateOptions(string channelpath)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                Console.WriteLine($"Message failed to {Name} channel {channelpath} {args.Exception}");
                return Task.CompletedTask;
            };

            var options = new MessageHandlerOptions(handler) { AutoComplete = false };

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

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration, ILogger logger, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null)
            : base(factory, configuration, logger)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}