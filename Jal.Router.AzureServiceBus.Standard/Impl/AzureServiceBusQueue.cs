using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusQueue : AzureServiceBus, IPointToPointChannel
    {
        private QueueClient _client;

        public void Open(SenderContext sendercontext)
        {
            _client = new QueueClient(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);

            var connection = Get(sendercontext.Channel);

            if(connection.TimeoutInSeconds>0)
            {
                _client.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(connection.TimeoutInSeconds);
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

                var properties = new AzureServiceBusChannelProperties(channel.Properties);

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(properties.DefaultMessageTtlInDays);

                description.LockDuration = TimeSpan.FromSeconds(properties.MessageLockDurationInSeconds);

                if (properties.DuplicateMessageDetectionInMinutes>0)
                {
                    description.RequiresDuplicateDetection = true;

                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(properties.DuplicateMessageDetectionInMinutes);
                }

                if (properties.SessionEnabled!=null)
                {
                    description.RequiresSession = properties.SessionEnabled.Value;
                }

                if (properties.PartitioningEnabled!=null)
                {
                    description.EnablePartitioning = properties.PartitioningEnabled.Value;
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

        public AzureServiceBusQueue(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider) 
            : base(factory, logger, provider)
        {
            
        }
    }
}