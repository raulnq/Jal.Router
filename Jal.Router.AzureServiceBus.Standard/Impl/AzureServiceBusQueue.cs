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
    public class AzureServiceBusQueue : AbstractChannel, IPointToPointChannel
    {
        private QueueClient _client;

        public void Open(SenderContext sendercontext)
        {
            _client = new QueueClient(sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);

            if(_parameter.TimeoutInSeconds>0)
            {
                _client.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                await _client.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public void Open(ListenerContext listenercontext)
        {
            _client = new QueueClient(listenercontext.Channel.ToConnectionString, listenercontext.Channel.ToPath);
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return !_client.IsClosedOrClosing;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return !_client.IsClosedOrClosing;
        }

        public Task Close(SenderContext sendercontext)
        {
            return _client.CloseAsync();
        }

        public void Listen(ListenerContext listenercontext)
        {
            var options = CreateOptions(listenercontext);

            var sessionoptions = CreateSessionOptions(listenercontext);

            var adapter = Factory.CreateMessageAdapter();

            if (listenercontext.Partition != null)
            {
                _client.RegisterSessionHandler(async (ms, message, token) => {

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

                        if (listenercontext.Partition.Until(context))
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
                _client.RegisterMessageHandler(async (message, token) =>
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

                        await _client.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
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

        public Task Close(ListenerContext context)
        {
            return _client.CloseAsync();
        }

        private readonly AzureServiceBusParameter _parameter;

        public AzureServiceBusQueue(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider) 
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusParameter>();
        }
    }
}