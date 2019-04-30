using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusTopic : AbstractChannel, IPublishSubscribeChannel
    {
        private SubscriptionClient _subscriptionclient;

        private TopicClient _topicclient;

        private ListenerMetadata _listenermetadata;

        private SenderMetadata _sendermetadata;

        public void Open(ListenerMetadata metadata)
        {
            _listenermetadata = metadata;

            _subscriptionclient = new SubscriptionClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath, metadata.Channel.ToSubscription);
        }

        public bool IsActive()
        {
            return _subscriptionclient != null ? !_subscriptionclient.IsClosedOrClosing : _topicclient.IsClosedOrClosing;
        }

        public Task Close()
        {
            return _subscriptionclient != null ?_subscriptionclient.CloseAsync() : _topicclient.CloseAsync();
        }

        public void Listen()
        {
            var options = CreateOptions(_listenermetadata);

            var sessionoptions = CreateSessionOptions(_listenermetadata);

            var adapter = Factory.CreateMessageAdapter();

            if (_listenermetadata.Group != null)
            {
                _subscriptionclient.RegisterSessionHandler(async (ms, message, token) => {

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
                _subscriptionclient.RegisterMessageHandler(async (message, token) =>
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

                        await _subscriptionclient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
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
            };

            var options = new MessageHandlerOptions(handler) { AutoComplete = false };

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

        public void Open(SenderMetadata metadata)
        {
            _sendermetadata = metadata;

            _topicclient = new TopicClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);

            if (_parameter.TimeoutInSeconds > 0)
            {
                _topicclient.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                await _topicclient.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        private readonly AzureServiceBusParameter _parameter;

        public AzureServiceBusTopic(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusParameter>();
        }
    }
}