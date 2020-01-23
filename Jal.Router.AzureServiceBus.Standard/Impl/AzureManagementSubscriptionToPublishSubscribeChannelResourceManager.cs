using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Rest.Azure;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureManagementSubscriptionToPublishSubscribeChannelResourceManager : AbstractAzureManagementChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>
    {
        public AzureManagementSubscriptionToPublishSubscribeChannelResourceManager(IComponentFactoryGateway factory) : base(factory)
        {
        }

        public override async Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    try
                    {
                        await topic.Subscriptions.GetByNameAsync(channel.Subscription).ConfigureAwait(false);

                        return false;
                    }
                    catch (CloudException ex)
                    {
                        if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                        {
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

                            var descriptor = topic.Subscriptions.Define(channel.Subscription)
                                .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                                .WithMessageLockDurationInSeconds(lockduration);

                            if (channel.Properties.ContainsKey(SessionEnabled))
                            {
                                descriptor = descriptor.WithSession();
                            }

                            await descriptor.CreateAsync().ConfigureAwait(false);

                            var subs = new Microsoft.Azure.ServiceBus.SubscriptionClient(configuration.ConnectionString, channel.Path, channel.Subscription);

                            var rule = channel.Rules.FirstOrDefault();

                            if (rule != null)
                            {
                                await subs.RemoveRuleAsync("$Default").ConfigureAwait(false);

                                var ruledescriptor = new RuleDescription(rule.Name, new SqlFilter(rule.Filter));

                                await subs.AddRuleAsync(ruledescriptor).ConfigureAwait(false);
                            }

                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public override async Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    try
                    {
                        await topic.Subscriptions.DeleteByNameAsync(channel.Subscription).ConfigureAwait(false);

                        return true;
                    }
                    catch (CloudException)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public override async Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    var subs = await topic.Subscriptions.GetByNameAsync(channel.Subscription).ConfigureAwait(false);

                    var statistics = new SubscriptionToPublishSubscribeChannelStatistics(channel.Subscription, channel.Path);

                    statistics.Properties.Add("DeadLetterMessageCount", subs.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", subs.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", subs.ScheduledMessageCount.ToString());

                    return statistics;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }
    }
}