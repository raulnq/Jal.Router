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
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override string Send(MessageContext context, object message)
        {
            var queueclient = new QueueClient(context.ToConnectionString, context.ToPath);

            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                queueclient.SendAsync(sbmessage).GetAwaiter().GetResult();

                queueclient.CloseAsync().GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public override void Listen(Route route, Action<object>[] routingactions, string channelpath)
        {
            var queueclient = new QueueClient(route.ToConnectionString, route.ToPath);

            var options = CreateOptions(channelpath);

            Action<Message> @action = message =>
            {
                foreach (var routingaction in routingactions)
                {
                    var clone = message.Clone();

                    routingaction(clone);
                }
            };

            queueclient.RegisterMessageHandler(async (message, token) =>
            {
                await OnMessageAsync(channelpath, message.MessageId, () => @action(message), () => queueclient.CompleteAsync(message.SystemProperties.LockToken));
            }, options);

            route.ShutdownAction = () => { queueclient.CloseAsync().GetAwaiter().GetResult(); };
        }

        private MessageHandlerOptions CreateOptions(string channelpath)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                Console.WriteLine($"Message failed to {ChannelName} channel {channelpath} {args.Exception}");
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

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null) 
            : base(factory, configuration, builder)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}