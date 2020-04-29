using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Rest.Azure;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureManagementPointToPointResource : AbstractAzureManagementResource
    {
        public AzureManagementPointToPointResource(IComponentFactoryFacade factory) : base(factory)
        {
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
                    var queue = await serviceBusNamespace.Queues.GetByNameAsync(resource.Path).ConfigureAwait(false);

                    var statistics = new Statistic(resource.Path);

                    statistics.Properties.Add("DeadLetterMessageCount", queue.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", queue.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", queue.ScheduledMessageCount.ToString());

                    statistics.Properties.Add("CurrentSizeInBytes", queue.CurrentSizeInBytes.ToString());

                    return statistics;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
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
                    await serviceBusNamespace.Queues.GetByNameAsync(resource.Path).ConfigureAwait(false);

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

                        var descriptor = serviceBusNamespace.Queues.Define(resource.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                            .WithMessageLockDurationInSeconds(lockduration);

                        if (resource.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(resource.Properties[DuplicateMessageDetectionInMinutes]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
                        }

                        if (resource.Properties.ContainsKey(SessionEnabled))
                        {
                            descriptor = descriptor.WithSession();
                        }

                        if (resource.Properties.ContainsKey(PartitioningEnabled))
                        {
                            descriptor = descriptor.WithPartitioning();
                        }

                        if (resource.Properties.ContainsKey(ExpressMessageEnabled))
                        {
                            descriptor = descriptor.WithExpressMessage();
                        }

                        await descriptor.CreateAsync().ConfigureAwait(false);

                        return true;
                    }
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
                    await serviceBusNamespace.Queues.DeleteByNameAsync(resource.Path).ConfigureAwait(false);

                    return true;
                }
                catch (CloudException)
                {
                    return false;
                }
            }

            return false;
        }
    }
}