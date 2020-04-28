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
    public class AzureManagementSubscriptionToPublishSubscribeResource : AbstractAzureManagementResource
    {
        public AzureManagementSubscriptionToPublishSubscribeResource(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var configuration = context.MessageSerializer.Deserialize<AzureServiceBusConfiguration>(resource.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(resource.Path).ConfigureAwait(false);

                    try
                    {
                        await topic.Subscriptions.GetByNameAsync(resource.Subscription).ConfigureAwait(false);

                        return false;
                    }
                    catch (CloudException ex)
                    {
                        if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                        {
                            var messagettl = 14;

                            var lockduration = 300;

                            if (resource.Properties.ContainsKey(DefaultMessageTtlInDays))
                            {
                                messagettl = Convert.ToInt32(resource.Properties[DefaultMessageTtlInDays]);
                            }

                            if (resource.Properties.ContainsKey(MessageLockDurationInSeconds))
                            {
                                lockduration = Convert.ToInt32(resource.Properties[MessageLockDurationInSeconds]);
                            }

                            var descriptor = topic.Subscriptions.Define(resource.Subscription)
                                .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                                .WithMessageLockDurationInSeconds(lockduration);

                            if (resource.Properties.ContainsKey(SessionEnabled))
                            {
                                descriptor = descriptor.WithSession();
                            }

                            await descriptor.CreateAsync().ConfigureAwait(false);

                            var subs = new Microsoft.Azure.ServiceBus.SubscriptionClient(configuration.ConnectionString, resource.Path, resource.Subscription);

                            var rule = resource.Rules.FirstOrDefault();

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

        public override async Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var configuration = context.MessageSerializer.Deserialize<AzureServiceBusConfiguration>(resource.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(resource.Path).ConfigureAwait(false);

                    try
                    {
                        await topic.Subscriptions.DeleteByNameAsync(resource.Subscription).ConfigureAwait(false);

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

        public override async Task<Statistic> Get(ResourceContext context)
        {
            var resource = context.Resource;

            var configuration = context.MessageSerializer.Deserialize<AzureServiceBusConfiguration>(resource.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(resource.Path).ConfigureAwait(false);

                    var subs = await topic.Subscriptions.GetByNameAsync(resource.Subscription).ConfigureAwait(false);

                    var statistics = new Statistic(resource.Subscription, resource.Path);

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