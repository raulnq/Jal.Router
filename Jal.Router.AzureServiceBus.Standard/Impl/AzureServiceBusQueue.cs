using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
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

        public Func<object[], bool> IsActiveMethodFactory(ListenerMetadata metadata)
        {
            return listener => {
                var client = listener[0] as QueueClient;
                return !client.IsClosedOrClosing;
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

            var sessionoptions = CreateSessionOptions(metadata);

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            Action<Message, MessageContext> handler = (message, context) =>
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

                if(metadata.Group!=null)
                {
                    client.RegisterSessionHandler(async (ms, message, token) => {

                        var context=adapter.ReadMetadata(message);

                        await OnMessageAsync(metadata, context, () => handler(message, context), () => ms.CompleteAsync(message.SystemProperties.LockToken), ()=> ms.CloseAsync());

                    }, sessionoptions);
                }
                else
                {
                    client.RegisterMessageHandler(async (message, token) =>
                    {
                        var context = adapter.ReadMetadata(message);

                        await OnMessageAsync(metadata, context, () => handler(message, context), () => client.CompleteAsync(message.SystemProperties.LockToken));

                    }, options);
                }
            };
        }

        private SessionHandlerOptions CreateSessionOptions(ListenerMetadata metadata)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                Logger.Log($"Message failed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()} Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} Executing Action: {context.Action}, {args.Exception}");

                return Task.CompletedTask;
            };

            var options = new SessionHandlerOptions(handler) { AutoComplete = false };

            if (_parameter.MaxConcurrentGroups > 0)
            {
                options.MaxConcurrentSessions = _parameter.MaxConcurrentGroups;
            }
            if (_parameter.AutoRenewGroupTimeoutInSeconds > 0)
            {
                options.MaxAutoRenewDuration = TimeSpan.FromSeconds(_parameter.AutoRenewGroupTimeoutInSeconds);
            }
            if (_parameter.MessageGroupTimeoutInSeconds > 0)
            {
                options.MessageWaitTimeout = TimeSpan.FromSeconds(_parameter.MessageGroupTimeoutInSeconds);
            }
            return options;
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

            if (_parameter.MaxConcurrentCalls > 0)
            {
                options.MaxConcurrentCalls = _parameter.MaxConcurrentCalls;
            }
            if (_parameter.AutoRenewTimeoutInMinutes > 0)
            {
                options.MaxAutoRenewDuration = TimeSpan.FromMinutes(_parameter.AutoRenewTimeoutInMinutes);
            }
            return options;
        }

        private readonly AzureServiceBusParameter _parameter;

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration, ILogger logger, IParameterProvider provider) 
            : base(factory, configuration, logger)
        {
            _parameter = provider.Get<AzureServiceBusParameter>();
        }
    }
}