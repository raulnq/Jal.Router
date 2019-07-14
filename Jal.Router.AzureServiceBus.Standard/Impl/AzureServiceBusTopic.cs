using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusTopic : AbstractChannel, IPublishSubscribeChannel
    {
        private SubscriptionClient _subscriptionclient;

        private TopicClient _topicclient;

        public void Open(ListenerContext listenercontext)
        {
            _subscriptionclient = new SubscriptionClient(listenercontext.Channel.ToConnectionString, listenercontext.Channel.ToPath, listenercontext.Channel.ToSubscription);
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return !_subscriptionclient.IsClosedOrClosing;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return !_topicclient.IsClosedOrClosing;
        }

        public Task Close(ListenerContext listenercontext)
        {
            return _subscriptionclient.CloseAsync();
        }

        public Task Close(SenderContext sendercontext)
        {
            return _topicclient.CloseAsync();
        }

        public void Listen(ListenerContext listenercontext)
        {
            var options = CreateOptions(listenercontext);

            var sessionoptions = CreateSessionOptions(listenercontext);

            var adapter = Factory.CreateMessageAdapter();

            if (listenercontext.Group != null)
            {
                _subscriptionclient.RegisterSessionHandler(async (ms, message, token) => {

                    var context = adapter.ReadMetadata(message);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in listenercontext.Routes.Select(x => x.RuntimeHandler))
                        {
                            var clone = message.Clone();

                            handlers.Add(runtimehandler(clone, listenercontext.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        await ms.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

                        if (listenercontext.Group.Until(context))
                        {
                            await ms.CloseAsync().ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");
                    }

                }, sessionoptions);
            }
            else
            {
                _subscriptionclient.RegisterMessageHandler(async (message, token) =>
                {
                    var context = adapter.ReadMetadata(message);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in listenercontext.Routes.Select(x => x.RuntimeHandler))
                        {
                            var clone = message.Clone();

                            handlers.Add(runtimehandler(clone, listenercontext.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        await _subscriptionclient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");
                    }

                }, options);
            }
        }

        private SessionHandlerOptions CreateSessionOptions(ListenerContext listenercontext)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                Logger.Log($"Message failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} Executing Action: {context.Action}, {args.Exception}");

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

        private MessageHandlerOptions CreateOptions(ListenerContext listenercontext)
        {
            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                Logger.Log($"Message failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} Executing Action: {context.Action}, {args.Exception}");

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

        public void Open(SenderContext sendercontext)
        {
            _topicclient = new TopicClient(sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);

            if (_parameter.TimeoutInSeconds > 0)
            {
                _topicclient.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
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