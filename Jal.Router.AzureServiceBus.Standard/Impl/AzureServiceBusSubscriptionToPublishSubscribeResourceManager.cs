using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusSubscriptionToPublishSubscribeResourceManager : AbstractAzureServiceBusResourceManager
    {
        public AzureServiceBusSubscriptionToPublishSubscribeResourceManager(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(Resource channel)
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

        public override async Task<bool> CreateIfNotExist(Resource channel)
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

        public override async Task<bool> DeleteIfExist(Resource channel)
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
}