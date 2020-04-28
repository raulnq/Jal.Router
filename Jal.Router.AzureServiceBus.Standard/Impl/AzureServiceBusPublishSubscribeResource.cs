using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusPublishSubscribeResource : AbstractAzureServiceBusResource
    {
        public AzureServiceBusPublishSubscribeResource(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.TopicExistsAsync(resource.Path).ConfigureAwait(false))
            {
                var info = await client.GetTopicRuntimeInfoAsync(resource.Path).ConfigureAwait(false);

                var statistics = new Statistic(resource.Path);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                return statistics;
            }

            return null;
        }

        public override async Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.TopicExistsAsync(resource.Path).ConfigureAwait(false))
            {
                await client.DeleteTopicAsync(resource.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public override async Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (!await client.TopicExistsAsync(resource.Path).ConfigureAwait(false))
            {
                var description = new TopicDescription(resource.Path)
                {
                    SupportOrdering = true
                };

                var messagettl = 14;

                if (resource.Properties.ContainsKey(DefaultMessageTtlInDays))
                {
                    messagettl = Convert.ToInt32(resource.Properties[DefaultMessageTtlInDays]);
                }

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                if (resource.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                {
                    var duplicatemessagedetectioninminutes = Convert.ToInt32(resource.Properties[DuplicateMessageDetectionInMinutes]);
                    description.RequiresDuplicateDetection = true;
                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                }

                if (resource.Properties.ContainsKey(PartitioningEnabled))
                {
                    description.EnablePartitioning = true;
                }

                if (resource.Properties.ContainsKey(ExpressMessageEnabled))
                {
                    
                }

                await client.CreateTopicAsync(description).ConfigureAwait(false);

                return true;
            }

            return false;
        }
    }
}