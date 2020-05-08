using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusQueue : AbstractChannel, IPointToPointChannel
    {
        private QueueClient _client;

        public void Open(SenderContext sendercontext)
        {
            _client = new QueueClient(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);

            if(_parameter.TimeoutInSeconds>0)
            {
                _client.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

            if (sbmessage != null)
            {
                await _client.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public void Open(ListenerContext listenercontext)
        {
            _client = new QueueClient(listenercontext.Channel.ConnectionString, listenercontext.Channel.Path);
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

            if (listenercontext.Channel.UsePartition)
            {
                _client.RegisterSessionHandler(async (ms, message, token) => {

                    var context = await listenercontext.Read(message).ConfigureAwait(false);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");

                    try
                    {
                        await listenercontext.Dispatch(context).ConfigureAwait(false);

                        await ms.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

                        if (listenercontext.Channel.ClosePartitionCondition!=null && listenercontext.Channel.ClosePartitionCondition(context))
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
                _client.RegisterMessageHandler(async (message, token) =>
                {
                    var context = await listenercontext.Read(message).ConfigureAwait(false);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");

                    try
                    {
                        await listenercontext.Dispatch(context).ConfigureAwait(false);

                        await _client.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} route {listenercontext.Route?.Name}");
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

        public async Task<bool> CreateIfNotExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (!await client.QueueExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var description = new QueueDescription(channel.Path);

                var messagettl = 14;

                var lockduration = 300;

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._DefaultMessageTtlInDays))
                {
                    messagettl = Convert.ToInt32(channel.Properties[AzureServiceBusChannelProperties._DefaultMessageTtlInDays]);
                }

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._MessageLockDurationInSeconds))
                {
                    lockduration = Convert.ToInt32(channel.Properties[AzureServiceBusChannelProperties._MessageLockDurationInSeconds]);
                }

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                description.LockDuration = TimeSpan.FromSeconds(lockduration);

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._DuplicateMessageDetectionInMinutes))
                {
                    var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[AzureServiceBusChannelProperties._DuplicateMessageDetectionInMinutes]);
                    description.RequiresDuplicateDetection = true;
                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                }

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._SessionEnabled))
                {
                    description.RequiresSession = true;
                }

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._PartitioningEnabled))
                {
                    description.EnablePartitioning = true;
                }

                await client.CreateQueueAsync(description).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public async Task<Statistic> GetStatistic(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.QueueExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var info = await client.GetQueueRuntimeInfoAsync(channel.Path).ConfigureAwait(false);

                var statistics = new Statistic(channel.Path);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                return statistics;
            }

            return null;
        }

        public async Task<bool> DeleteIfExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.QueueExistsAsync(channel.Path).ConfigureAwait(false))
            {
                await client.DeleteQueueAsync(channel.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public async Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            var client = default(SessionClient);

            if(sendercontext.Channel.ReplyChannel.ChannelType== ChannelType.PointToPoint)
            {
                client = new SessionClient(sendercontext.Channel.ReplyChannel.ConnectionString, sendercontext.Channel.ReplyChannel.Path);
            }
            else
            {
                var entity = EntityNameHelper.FormatSubscriptionPath(sendercontext.Channel.ReplyChannel.Path, sendercontext.Channel.ReplyChannel.Subscription);

                client = new SessionClient(sendercontext.Channel.ReplyChannel.ConnectionString, entity);
            }
            
            var messagesession = await client.AcceptMessageSessionAsync(context.TracingContext.ReplyToRequestId).ConfigureAwait(false);

            var message = sendercontext.Channel.ReplyTimeOut != 0 ?
                await messagesession.ReceiveAsync(TimeSpan.FromSeconds(sendercontext.Channel.ReplyTimeOut)).ConfigureAwait(false) :
                await messagesession.ReceiveAsync().ConfigureAwait(false);

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = await adapter.ReadFromPhysicalMessage(message, sendercontext).ConfigureAwait(false);

                await messagesession.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }

            await messagesession.CloseAsync().ConfigureAwait(false);

            await client.CloseAsync().ConfigureAwait(false);

            return outputcontext;
        }

        private readonly AzureServiceBusChannelConnection _parameter;

        public AzureServiceBusQueue(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider) 
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusChannelConnection>();
        }
    }
}