﻿using System;
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
    public class AzureServiceBusSubscription : AbstractChannel, ISubscriptionToPublishSubscribeChannel
    {
        private SubscriptionClient _subscriptionclient;

        public void Open(ListenerContext listenercontext)
        {
            _subscriptionclient = new SubscriptionClient(listenercontext.Channel.ConnectionString, listenercontext.Channel.Path, listenercontext.Channel.Subscription);
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return !_subscriptionclient.IsClosedOrClosing;
        }

        public Task Close(ListenerContext listenercontext)
        {
            return _subscriptionclient.CloseAsync();
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

        public async Task<bool> CreateIfNotExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (!await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
            {
                var description = new SubscriptionDescription(channel.Path, channel.Subscription);

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

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._SessionEnabled))
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

        public async Task<Statistic> GetStatistic(Channel channel)
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

        public async Task<bool> DeleteIfExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
            {
                await client.DeleteSubscriptionAsync(channel.Path, channel.Subscription).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        private readonly AzureServiceBusChannelConnection _parameter;

        public AzureServiceBusSubscription(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusChannelConnection>();
        }
    }
}