using System;
using System.Net;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Rest.Azure;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureManagementPublishSubscribeChannelResource : AbstractAzureManagementChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>
    {
        public AzureManagementPublishSubscribeChannelResource(IComponentFactoryGateway factory) : base(factory)
        {
        }

        public override async Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var messagettl = 14;

                        if (channel.Properties.ContainsKey(DefaultMessageTtlInDays))
                        {
                            messagettl = Convert.ToInt32(channel.Properties[DefaultMessageTtlInDays]);
                        }

                        var descriptor = serviceBusNamespace.Topics.Define(channel.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl));

                        if (channel.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[DuplicateMessageDetectionInMinutes]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
                        }

                        if (channel.Properties.ContainsKey(PartitioningEnabled))
                        {
                            descriptor = descriptor.WithPartitioning();
                        }

                        if (channel.Properties.ContainsKey(ExpressMessageEnabled))
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

        public override async Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    await serviceBusNamespace.Topics.DeleteByNameAsync(channel.Path).ConfigureAwait(false);

                    return true;
                }
                catch (CloudException)
                {
                    return false;
                }
            }

            return false;
        }

        public override async Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    var statistics = new PublishSubscribeChannelStatistics(channel.Path);

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