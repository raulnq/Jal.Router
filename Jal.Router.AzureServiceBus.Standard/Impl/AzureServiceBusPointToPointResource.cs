using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Threading.Tasks;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusPointToPointResource : AbstractAzureServiceBusResource
    {
        public AzureServiceBusPointToPointResource(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.QueueExistsAsync(resource.Path).ConfigureAwait(false))
            {
                var info = await client.GetQueueRuntimeInfoAsync(resource.Path).ConfigureAwait(false);

                var statistics = new Statistic(resource.Path);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                return statistics;
            }

            return null;
        }

        public override async Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if(!await client.QueueExistsAsync(resource.Path).ConfigureAwait(false))
            {
                var description = new QueueDescription(resource.Path);

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

                if (resource.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                {
                    var duplicatemessagedetectioninminutes = Convert.ToInt32(resource.Properties[DuplicateMessageDetectionInMinutes]);
                    description.RequiresDuplicateDetection = true;
                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                }

                if (resource.Properties.ContainsKey(SessionEnabled))
                {
                    description.RequiresSession = true;
                }

                if (resource.Properties.ContainsKey(PartitioningEnabled))
                {
                    description.EnablePartitioning = true;
                }

                await client.CreateQueueAsync(description).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public override async Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var client = new ManagementClient(resource.ConnectionString);

            if (await client.QueueExistsAsync(resource.Path).ConfigureAwait(false))
            {
                await client.DeleteQueueAsync(resource.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }
    }
}