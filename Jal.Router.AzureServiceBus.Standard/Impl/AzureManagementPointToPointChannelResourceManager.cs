using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Rest.Azure;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureManagementPointToPointChannelResourceManager : AbstractAzureManagementChannelResourceManager
    {
        public AzureManagementPointToPointChannelResourceManager(IComponentFactoryFacade factory) : base(factory)
        {
        }

        public override async Task<Statistic> Get(Resource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var queue = await serviceBusNamespace.Queues.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    var statistics = new Statistic(channel.Path);

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

        public override async Task<bool> CreateIfNotExist(Resource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    await serviceBusNamespace.Queues.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
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

                        var descriptor = serviceBusNamespace.Queues.Define(channel.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                            .WithMessageLockDurationInSeconds(lockduration);

                        if (channel.Properties.ContainsKey(DuplicateMessageDetectionInMinutes))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[DuplicateMessageDetectionInMinutes]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
                        }

                        if (channel.Properties.ContainsKey(SessionEnabled))
                        {
                            descriptor = descriptor.WithSession();
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

        public override async Task<bool> DeleteIfExist(Resource channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    await serviceBusNamespace.Queues.DeleteByNameAsync(channel.Path).ConfigureAwait(false);

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