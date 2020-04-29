using System;
using System.Net;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Rest.Azure;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureManagementPublishSubscribeResource : AbstractAzureManagementResource
    {
        public AzureManagementPublishSubscribeResource(IComponentFactoryFacade factory) : base(factory)
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
                    await serviceBusNamespace.Topics.GetByNameAsync(resource.Path).ConfigureAwait(false);

                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var messagettl = 14;

                        if (resource.Properties.ContainsKey(DefaultMessageTtlInDays))
                        {
                            messagettl = Convert.ToInt32(resource.Properties[DefaultMessageTtlInDays]);
                        }

                        var descriptor = serviceBusNamespace.Topics.Define(resource.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl));

                        if (resource.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(resource.Properties[DuplicateMessageDetectionInMinutes]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
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
                    await serviceBusNamespace.Topics.DeleteByNameAsync(resource.Path).ConfigureAwait(false);

                    return true;
                }
                catch (CloudException)
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

                    var statistics = new Statistic(resource.Path);

                    statistics.Properties.Add("DeadLetterMessageCount", topic.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", topic.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", topic.ScheduledMessageCount.ToString());

                    statistics.Properties.Add("CurrentSizeInBytes", topic.CurrentSizeInBytes.ToString());

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