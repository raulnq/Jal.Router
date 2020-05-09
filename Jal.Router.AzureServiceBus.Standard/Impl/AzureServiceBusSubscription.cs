using System;
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
    public class AzureServiceBusSubscription : AzureServiceBus, ISubscriptionToPublishSubscribeChannel
    {
        private SubscriptionClient _subscriptionclient;

        public void Open(ListenerContext listenercontext)
        {
            _subscriptionclient = new SubscriptionClient(listenercontext.Channel.ConnectionString, listenercontext.Channel.Path, listenercontext.Channel.Subscription);

            var connection = Get(listenercontext.Channel);

            if (connection.TimeoutInSeconds > 0)
            {
                _subscriptionclient.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(connection.TimeoutInSeconds);
            }
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

        public async Task<bool> CreateIfNotExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (!await client.SubscriptionExistsAsync(channel.Path, channel.Subscription).ConfigureAwait(false))
            {
                var description = new SubscriptionDescription(channel.Path, channel.Subscription);

                var properties = new AzureServiceBusChannelProperties(channel.Properties);

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(properties.DefaultMessageTtlInDays);

                description.LockDuration = TimeSpan.FromSeconds(properties.MessageLockDurationInSeconds);

                if (properties.SessionEnabled != null)
                {
                    description.RequiresSession = properties.SessionEnabled.Value;
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

        public AzureServiceBusSubscription(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger, provider)
        {

        }
    }
}