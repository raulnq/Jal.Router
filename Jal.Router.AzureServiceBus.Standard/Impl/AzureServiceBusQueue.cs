using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusQueue : AbstractChannel, IPointToPointChannel
    {

        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath) };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return sender =>
            {
                var client = sender[0] as QueueClient;
                
                client.CloseAsync().GetAwaiter().GetResult();
            };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (sender, message) =>
            {
                var client = sender[0] as QueueClient;

                var sbmessage = message as Message;

                if (sbmessage != null)
                {
                    client.SendAsync(sbmessage).GetAwaiter().GetResult();

                    return sbmessage.MessageId;
                }

                return string.Empty;
            };
        }

     

        public Func<object[]> CreateListenerMethodFactory(ListenerMetadata metadata)
        {
            return () =>
            {

                var client = new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);

                return new object[] { client };
            };
        }

        public Action<object[]> DestroyListenerMethodFactory(ListenerMetadata metadata)
        {
            return listener =>
            {
                var client = listener[0] as QueueClient;

                client.CloseAsync().GetAwaiter().GetResult();
            };
        }

        public Action<object[]> ListenerMethodFactory(ListenerMetadata metadata)
        {
            var options = CreateOptions(metadata);

            Action<Message> handler = message =>
            {
                foreach (var runtimehandler in metadata.Routes.Select(x => x.RuntimeHandler))
                {
                    var clone = message.Clone();

                    runtimehandler(clone, metadata.Channel);
                }
            };

            return (listener) =>
            {
                var client = listener[0] as QueueClient;

                client.RegisterMessageHandler(async (message, token) =>
                {
                    await OnMessageAsync(metadata, message.MessageId, () => handler(message), () => client.CompleteAsync(message.SystemProperties.LockToken));
                }, options);

            };
        }

        private MessageHandlerOptions CreateOptions(ListenerMetadata metadata)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                Logger.Log($"Message failed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()} Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} Executing Action: {context.Action}, {args.Exception}");

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