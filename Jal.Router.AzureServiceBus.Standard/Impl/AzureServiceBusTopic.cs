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
            _subscriptionclient = new SubscriptionClient(listenercontext.Channel.ConnectionString, listenercontext.Channel.Path, listenercontext.Channel.Subscription);
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

            if (listenercontext.Channel.Partition)
            {
                _subscriptionclient.RegisterSessionHandler(async (ms, message, token) => {

                    var context = await listenercontext.Read(message).ConfigureAwait(false);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");

                    try
                    {
                        await listenercontext.Dispatch(context).ConfigureAwait(false);

                        await ms.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

                        if (listenercontext.Channel.ClosePartitionCondition != null && listenercontext.Channel.ClosePartitionCondition(context))
                        {
                            await ms.CloseAsync().ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");
                    }

                }, sessionoptions);
            }
            else
            {
                _subscriptionclient.RegisterMessageHandler(async (message, token) =>
                {
                    var context = await listenercontext.Read(message).ConfigureAwait(false);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");

                    try
                    {
                        await listenercontext.Dispatch(context).ConfigureAwait(false);

                        await _subscriptionclient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name} {ex}");
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

            if (_parameter.MaxConcurrentPartitions > 0)
            {
                options.MaxConcurrentSessions = _parameter.MaxConcurrentPartitions;
            }
            if (_parameter.AutoRenewPartitionTimeoutInSeconds > 0)
            {
                options.MaxAutoRenewDuration = TimeSpan.FromSeconds(_parameter.AutoRenewPartitionTimeoutInSeconds);
            }
            if (_parameter.MessagePartitionTimeoutInSeconds > 0)
            {
                options.MessageWaitTimeout = TimeSpan.FromSeconds(_parameter.MessagePartitionTimeoutInSeconds);
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
            _topicclient = new TopicClient(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);

            if (_parameter.TimeoutInSeconds > 0)
            {
                _topicclient.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

            if (sbmessage != null)
            {
                await _topicclient.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        private readonly AzureServiceBusParameter _parameter;

        public AzureServiceBusTopic(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusParameter>();
        }
    }
}