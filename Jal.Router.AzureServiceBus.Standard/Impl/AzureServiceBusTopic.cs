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

            if (listenercontext.Channel.UsePartition)
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

        public async Task<bool> CreateIfNotExist(Channel channel)
        {
            if(string.IsNullOrEmpty(channel.Subscription))
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (!await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
                {
                    var description = new TopicDescription(channel.Path)
                    {
                        SupportOrdering = true
                    };

                    var messagettl = 14;

                    if (channel.Properties.ContainsKey(DefaultMessageTtlInDays))
                    {
                        messagettl = Convert.ToInt32(channel.Properties[DefaultMessageTtlInDays]);
                    }

                    description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                    if (channel.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                    {
                        var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[DuplicateMessageDetectionInMinutes]);
                        description.RequiresDuplicateDetection = true;
                        description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                    }

                    if (channel.Properties.ContainsKey(PartitioningEnabled))
                    {
                        description.EnablePartitioning = true;
                    }

                    if (channel.Properties.ContainsKey(ExpressMessageEnabled))
                    {

                    }

                    await client.CreateTopicAsync(description).ConfigureAwait(false);

                    return true;
                }

                return false;
            }
            else
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (!await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
                {
                    var description = new SubscriptionDescription(channel.Path, channel.Subscription);

                    var messagettl = 14;

                    var lockduration = 300;

                    if (channel.Properties.ContainsKey(DefaultMessageTtlInDays))
                    {
                        messagettl = Convert.ToInt32(channel.Properties[DefaultMessageTtlInDays]);
                    }

                    if (channel.Properties.ContainsKey(MessageLockDurationInSeconds))
                    {
                        lockduration = Convert.ToInt32(channel.Properties[MessageLockDurationInSeconds]);
                    }

                    description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                    description.LockDuration = TimeSpan.FromSeconds(lockduration);

                    if (channel.Properties.ContainsKey(SessionEnabled))
                    {
                        description.RequiresSession = true;
                    }

                    await client.CreateSubscriptionAsync(description).ConfigureAwait(false);

                    var subs = new SubscriptionClient(channel.ConnectionString, channel.Path, channel.Subscription);

                    var rule = channel.Rules.FirstOrDefault();

                    if (rule != null)
                    {
                        await subs.RemoveRuleAsync("$Default").ConfigureAwait(false);

                        var ruledescriptor = new RuleDescription(rule.Name, new SqlFilter(rule.Filter));

                        await subs.AddRuleAsync(ruledescriptor).ConfigureAwait(false);
                    }

                    return true;
                }

                return false;
            }
        }

        public async Task<Statistic> GetStatistic(Channel channel)
        {
            if (string.IsNullOrEmpty(channel.Subscription))
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
                {
                    var info = await client.GetTopicRuntimeInfoAsync(channel.Path).ConfigureAwait(false);

                    var statistics = new Statistic(channel.Path);

                    statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                    statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                    return statistics;
                }

                return null;
            }
            else
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
                {
                    var info = await client.GetSubscriptionRuntimeInfoAsync(channel.Path, channel.Subscription).ConfigureAwait(false);

                    var statistics = new Statistic(channel.Path, channel.Subscription);

                    statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                    return statistics;
                }

                return null;
            }
        }

        public async Task<bool> DeleteIfExist(Channel channel)
        {
            if (string.IsNullOrEmpty(channel.Subscription))
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
                {
                    await client.DeleteTopicAsync(channel.Path).ConfigureAwait(false);

                    return true;
                }

                return false;
            }
            else
            {
                var client = new ManagementClient(channel.ConnectionString);

                if (await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
                {
                    await client.DeleteSubscriptionAsync(channel.Path, channel.Subscription).ConfigureAwait(false);

                    return true;
                }

                return false;
            }
        }

        public async Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            var client = default(SessionClient);

            if (sendercontext.Channel.ReplyType == ReplyType.FromPointToPoint)
            {
                client = new SessionClient(sendercontext.Channel.ReplyConnectionString, sendercontext.Channel.ReplyPath);
            }
            else
            {
                var entity = EntityNameHelper.FormatSubscriptionPath(sendercontext.Channel.ReplyPath, sendercontext.Channel.ReplySubscription);

                client = new SessionClient(sendercontext.Channel.ReplyConnectionString, entity);
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

        private readonly AzureServiceBusParameter _parameter;

        public AzureServiceBusTopic(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusParameter>();
        }
    }
}