using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusPublishSubscribeChannelResource : AbstractAzureServiceBusChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>
    {
        public AzureServiceBusPublishSubscribeChannelResource(IComponentFactoryGateway factory) : base(factory)
        {
        }

        public override async Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var info = await client.GetTopicRuntimeInfoAsync(channel.Path).ConfigureAwait(false);

                var statistics = new PublishSubscribeChannelStatistics(channel.Path);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                return statistics;
            }

            return null;
        }

        public override async Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                await client.DeleteTopicAsync(channel.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public override async Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (!await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var description = new TopicDescription(channel.Path);

                description.SupportOrdering = true;
                
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
    }
}