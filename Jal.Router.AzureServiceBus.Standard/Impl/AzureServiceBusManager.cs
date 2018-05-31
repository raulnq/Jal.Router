using System;
using System.Net;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusManager : IChannelManager
    {
        public bool CreateIfNotExistSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, string origin, bool all)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(path);

                    try
                    {
                       topic.Subscriptions.GetByName(subscription);

                       return false;
                    }
                    catch (CloudException ex)
                    {
                        if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                        {
                            topic.Subscriptions.Define(subscription)
                                .WithDefaultMessageTTL(TimeSpan.FromDays(14))
                                .WithMessageLockDurationInSeconds(300)
                                .Create();

                            var subs = new Microsoft.Azure.ServiceBus.SubscriptionClient(configuration.ConnectionString, path, subscription);

                            if (!all)
                            {
                                subs.RemoveRuleAsync("$Default").GetAwaiter().GetResult();

                                var ruledescriptor = new RuleDescription("$Default", new SqlFilter($"origin='{origin}'"));

                                subs.AddRuleAsync(ruledescriptor).GetAwaiter().GetResult();
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

        public bool CreateIfNotExistPublishSubscribeChannel(string connectionstring, string path)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    serviceBusNamespace.Topics.GetByName(path);

                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        serviceBusNamespace.Topics.Define(path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(14))
                            .Create();

                        return true;

                    }
                }
            }

            return false;
        }

        public PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string path)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(path);
                    
                    var info = new PublishSubscribeChannelInfo(path)
                    {
                        MessageCount = topic.ActiveMessageCount,
                        DeadLetterMessageCount = topic.DeadLetterMessageCount,
                        ScheduledMessageCount = topic.ScheduledMessageCount,
                        SizeInBytes = topic.CurrentSizeInBytes
                    };

                    return info;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;

        }

        public SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(path);

                    var subs = topic.Subscriptions.GetByName(subscription);
                    
                    var info = new SubscriptionToPublishSubscribeChannelInfo(subscription, path)
                    {
                        DeadLetterMessageCount = subs.DeadLetterMessageCount,
                        MessageCount = subs.ActiveMessageCount,
                        ScheduledMessageCount = subs.ScheduledMessageCount
                    };

                    return info;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string path)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var queue = serviceBusNamespace.Queues.GetByName(path);

                    var info = new PointToPointChannelInfo(path)
                    {
                        DeadLetterMessageCount = queue.DeadLetterMessageCount,
                        MessageCount = queue.ActiveMessageCount,
                        ScheduledMessageCount = queue.ScheduledMessageCount,
                        SizeInBytes = queue.CurrentSizeInBytes
                    };

                    return info;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public bool CreateIfNotExistPointToPointChannel(string connectionstring, string path)
        {
            var configuration = JsonConvert.DeserializeObject<ServiceBusConfiguration>(connectionstring);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    serviceBusNamespace.Queues.GetByName(path);
                    
                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        serviceBusNamespace.Queues.Define(path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(14))
                            .WithMessageLockDurationInSeconds(300)
                            .Create();

                        return true;
                    }

                }
            }

            return false;
        }

        private IServiceBusNamespace GetServiceBusNamespace(ServiceBusConfiguration configuration)
        {
            try
            {
                var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(configuration.ClientId, configuration.ClientSecret, configuration.TenantId, AzureEnvironment.AzureGlobalCloud);

                var serviceBusManager = ServiceBusManager.Authenticate(credentials, configuration.SubscriptionId);

                return serviceBusManager.Namespaces.GetByResourceGroup(configuration.ResourceGroupName, configuration.ResourceName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}