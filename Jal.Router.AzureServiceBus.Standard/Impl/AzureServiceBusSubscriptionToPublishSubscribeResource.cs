using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusSubscriptionToPublishSubscribeResource : AbstractAzureServiceBusResource
    {
        public AzureServiceBusSubscriptionToPublishSubscribeResource(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.SubscriptionExistsAsync(resource.Path, resource.Subscription).ConfigureAwait(false))
            {
                var info = await client.GetSubscriptionRuntimeInfoAsync(resource.Path, resource.Subscription).ConfigureAwait(false);

                var statistics = new Statistic(resource.Path, resource.Subscription);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                return statistics;
            }

            return null;
        }

        public override async Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (!await client.SubscriptionExistsAsync(resource.Path, resource.Subscription).ConfigureAwait(false))
            {
                var description = new SubscriptionDescription(resource.Path, resource.Subscription);

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

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                description.LockDuration = TimeSpan.FromSeconds(lockduration);

                if (resource.Properties.ContainsKey(SessionEnabled))
                {
                    description.RequiresSession = true;
                }

                await client.CreateSubscriptionAsync(description).ConfigureAwait(false);

                var subs = new SubscriptionClient(resource.ConnectionString, resource.Path, resource.Subscription);

                var rule = resource.Rules.FirstOrDefault();

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

        public override async Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.SubscriptionExistsAsync(resource.Path, resource.Subscription).ConfigureAwait(false))
            {
                await client.DeleteSubscriptionAsync(resource.Path, resource.Subscription).ConfigureAwait(false);

                return true;
            }

            return false;
        }
    }
}