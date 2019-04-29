using System;
using System.Collections.Generic;
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
        private QueueClient _client;

        private ListenerMetadata _listenermetadata;

        private SenderMetadata _sendermetadata;

        public void Open(SenderMetadata metadata)
        {
            _sendermetadata = metadata;

            _client = new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);

            if(_parameter.TimeoutInSeconds>0)
            {
                _client.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
            
        }

        public string Send(object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                _client.SendAsync(sbmessage).GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public void Open(ListenerMetadata metadata)
        {
            _listenermetadata = metadata;

            _client = new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);
        }

        public bool IsActive()
        {
            return !_client.IsClosedOrClosing;
        }

        public Task Close()
        {
            return _client.CloseAsync();
        }

        public void Listen()
        {
            var options = CreateOptions(_listenermetadata);

            var sessionoptions = CreateSessionOptions(_listenermetadata);

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            if (_listenermetadata.Group != null)
            {
                _client.RegisterSessionHandler(async (ms, message, token) => {

                    var context = adapter.ReadMetadata(message);

                    Logger.Log($"Message {context.IdentityContext.Id} arrived to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in _listenermetadata.Routes.Select(x => x.RuntimeHandler))
                        {
                            var clone = message.Clone();

                            handlers.Add(runtimehandler(clone, _listenermetadata.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        await ms.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

                        if (_listenermetadata.Group.Until(context))
                        {
                            await ms.CloseAsync().ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.IdentityContext.Id} failed to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.IdentityContext.Id} completed to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()}");
                    }

                }, sessionoptions);
            }
            else
            {
                _client.RegisterMessageHandler(async (message, token) =>
                {
                    var context = adapter.ReadMetadata(message);

                    Logger.Log($"Message {context.IdentityContext.Id} arrived to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in _listenermetadata.Routes.Select(x => x.RuntimeHandler))
                        {
                            var clone = message.Clone();

                            handlers.Add(runtimehandler(clone, _listenermetadata.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        await _client.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.IdentityContext.Id} failed to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.IdentityContext.Id} completed to {_listenermetadata.Channel.ToString()} channel {_listenermetadata.Channel.GetPath()}");
                    }
                }, options);
            }
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