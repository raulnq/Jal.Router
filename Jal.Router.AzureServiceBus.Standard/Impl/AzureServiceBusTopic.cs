using System;
using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
// ReSharper disable ConvertToLocalFunction

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusTopic : AbstractPublishSubscribeChannel
    {
        public override string Send(MessageContext context, object message)
        {
            var topicclient = new TopicClient(context.ToConnectionString, context.ToPath);

            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                topicclient.SendAsync(sbmessage).GetAwaiter().GetResult();

                topicclient.CloseAsync().GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(Channel channel, Action<object>[] routingactions, string channelpath)
        {
            var client = new SubscriptionClient(channel.ToConnectionString, channel.ToPath, channel.ToSubscription);

            var options = CreateOptions(channelpath);

            Action<Message> @action = message =>
            {
                foreach (var routingaction in routingactions)
                {
                    var clone = message.Clone();

                    routingaction(clone);
                }
            };


            client.RegisterMessageHandler(async (message, token) =>
            {
                await OnMessageAsync(channelpath, message.MessageId, () => @action(message), () => client.CompleteAsync(message.SystemProperties.LockToken));
            }, options);

            channel.ShutdownAction = () => { client.CloseAsync().GetAwaiter().GetResult(); };
        }

        private MessageHandlerOptions CreateOptions(string channelpath)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                Console.WriteLine($"Message failed to {ChannelName} channel {channelpath} {args.Exception}");
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

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null)
            : base(factory, configuration, builder)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}