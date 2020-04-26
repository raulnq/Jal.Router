using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Threading.Tasks;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusPointToPointResourceManager : AbstractAzureServiceBusResourceManager
    {
        public AzureServiceBusPointToPointResourceManager(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(Resource channel)
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

        public override async Task<bool> CreateIfNotExist(Resource channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if(!await client.QueueExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var description = new QueueDescription(channel.Path);

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

                if (channel.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                {
                    var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[DuplicateMessageDetectionInMinutes]);
                    description.RequiresDuplicateDetection = true;
                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                }

                if (channel.Properties.ContainsKey(SessionEnabled))
                {
                    description.RequiresSession = true;
                }

                if (channel.Properties.ContainsKey(PartitioningEnabled))
                {
                    description.EnablePartitioning = true;
                }

                await client.CreateQueueAsync(description).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public override async Task<bool> DeleteIfExist(Resource channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.QueueExistsAsync(channel.Path).ConfigureAwait(false))
            {
                await client.DeleteQueueAsync(channel.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }
    }
}