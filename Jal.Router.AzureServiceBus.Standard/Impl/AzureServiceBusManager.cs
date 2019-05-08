using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusManager : IChannelManager
    {
        public const string DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string SessionEnabled = "sessionenabled";

        public const string PartitioningEnabled = "partitioningenabled";

        public const string ExpressMessageEnabled = "expressmessageenabled";

        private readonly IComponentFactoryGateway _factory;

        public AzureServiceBusManager(IComponentFactoryGateway factory)
        {
            _factory = factory;

            LoggerCallbackHandler.UseDefaultLogging = false;
        }

        public async Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    try
                    {
                       await topic.Subscriptions.GetByNameAsync(channel.Subscription).ConfigureAwait(false);

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

                            var descriptor = topic.Subscriptions.Define(channel.Subscription)
                                .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                                .WithMessageLockDurationInSeconds(lockduration);

                            if (channel.Properties.ContainsKey(SessionEnabled))
                            {
                                descriptor = descriptor.WithSession();
                            }

                            await descriptor.CreateAsync().ConfigureAwait(false);

                            var subs = new Microsoft.Azure.ServiceBus.SubscriptionClient(configuration.ConnectionString, channel.Path, channel.Subscription);

                            var rule = channel.Rules.FirstOrDefault();

                            if (rule!=null)
                            {
                                await subs.RemoveRuleAsync("$Default").ConfigureAwait(false);

                                var ruledescriptor = new RuleDescription(rule.Name, new SqlFilter(rule.Filter));

                                await subs.AddRuleAsync(ruledescriptor).ConfigureAwait(false);
                            }

                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannel channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    try
                    {
                        await topic.Subscriptions.DeleteByNameAsync(channel.Subscription).ConfigureAwait(false);

                        return true;
                    }
                    catch (CloudException)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> CreateIfNotExist(PublishSubscribeChannel channel)
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

        public async Task<bool> DeleteIfExist(PublishSubscribeChannel channel)
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

        public async Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannel channel)
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

        public async Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannel channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = await serviceBusNamespace.Topics.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    var subs = await topic.Subscriptions.GetByNameAsync(channel.Subscription).ConfigureAwait(false);

                    var statistics = new SubscriptionToPublishSubscribeChannelStatistics(channel.Subscription, channel.Path);

                    statistics.Properties.Add("DeadLetterMessageCount", subs.DeadLetterMessageCount.ToString());

                    statistics.Properties.Add("ActiveMessageCount", subs.ActiveMessageCount.ToString());

                    statistics.Properties.Add("ScheduledMessageCount", subs.ScheduledMessageCount.ToString());

                    return statistics;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<PointToPointChannelStatistics> Get(PointToPointChannel channel)
        {
            var serializer = _factory.CreateMessageSerializer();

            var configuration = serializer.Deserialize<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = await GetServiceBusNamespace(configuration).ConfigureAwait(false);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var queue = await serviceBusNamespace.Queues.GetByNameAsync(channel.Path).ConfigureAwait(false);

                    var statistics = new PointToPointChannelStatistics(channel.Path);

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

        public async Task<bool> CreateIfNotExist(PointToPointChannel channel)
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
                        var messagettl =14;

                        var lockduration = 300;

                        if(channel.Properties.ContainsKey(DefaultMessageTtlInDays))
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

        public async Task<bool> DeleteIfExist(PointToPointChannel channel)
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

        private async Task<IServiceBusNamespace> GetServiceBusNamespace(AzureServiceBusConfiguration configuration)
        {
            try
            {
                var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(configuration.ClientId, configuration.ClientSecret, configuration.TenantId, AzureEnvironment.AzureGlobalCloud);

                var serviceBusManager = ServiceBusManager.Authenticate(credentials, configuration.SubscriptionId);

                return await serviceBusManager.Namespaces.GetByResourceGroupAsync(configuration.ResourceGroupName, 
                    configuration.ResourceName).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}