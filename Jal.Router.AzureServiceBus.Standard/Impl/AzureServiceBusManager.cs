using System;
using System.Linq;
using System.Net;
using Jal.Router.AzureServiceBus.Standard.Model;
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
        public AzureServiceBusManager()
        {
            LoggerCallbackHandler.UseDefaultLogging = false;
        }

        public bool CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(channel.Path);

                    try
                    {
                       topic.Subscriptions.GetByName(channel.Subscription);

                       return false;
                    }
                    catch (CloudException ex)
                    {
                        if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                        {
                            var messagettl = 14;

                            var lockduration = 300;

                            if (channel.Properties.ContainsKey("defaultmessagettlindays"))
                            {
                                messagettl = Convert.ToInt32(channel.Properties["defaultmessagettlindays"]);
                            }

                            if (channel.Properties.ContainsKey("messagelockdurationinseconds"))
                            {
                                lockduration = Convert.ToInt32(channel.Properties["messagelockdurationinseconds"]);
                            }

                            var descriptor = topic.Subscriptions.Define(channel.Subscription)
                                .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                                .WithMessageLockDurationInSeconds(lockduration);

                            if (channel.Properties.ContainsKey("sessionenabled"))
                            {
                                descriptor = descriptor.WithSession();
                            }

                            descriptor.Create();

                            var subs = new Microsoft.Azure.ServiceBus.SubscriptionClient(configuration.ConnectionString, channel.Path, channel.Subscription);

                            var rule = channel.Rules.FirstOrDefault();

                            if (rule!=null)
                            {
                                subs.RemoveRuleAsync("$Default").GetAwaiter().GetResult();

                                var ruledescriptor = new RuleDescription(rule.Name, new SqlFilter(rule.Filter));

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

        public bool CreateIfNotExist(PublishSubscribeChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    serviceBusNamespace.Topics.GetByName(channel.Path);

                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var messagettl = 14;

                        if (channel.Properties.ContainsKey("defaultmessagettlindays"))
                        {
                            messagettl = Convert.ToInt32(channel.Properties["defaultmessagettlindays"]);
                        } 

                        var descriptor = serviceBusNamespace.Topics.Define(channel.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl));

                        if (channel.Properties.ContainsKey("duplicatemessagedetectioninminutes"))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties["duplicatemessagedetectioninminutes"]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
                        }

                        if (channel.Properties.ContainsKey("partitioningenabled"))
                        {
                            descriptor = descriptor.WithPartitioning();
                        }

                        if (channel.Properties.ContainsKey("expressmessageenabled"))
                        {
                            descriptor = descriptor.WithExpressMessage();
                        }

                        descriptor.Create();

                        return true;

                    }
                }
            }

            return false;
        }

        public PublishSubscribeChannelInfo GetInfo(PublishSubscribeChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(channel.Path);
                    
                    var info = new PublishSubscribeChannelInfo(channel.Path)
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

        public SubscriptionToPublishSubscribeChannelInfo GetInfo(SubscriptionToPublishSubscribeChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var topic = serviceBusNamespace.Topics.GetByName(channel.Path);

                    var subs = topic.Subscriptions.GetByName(channel.Subscription);
                    
                    var info = new SubscriptionToPublishSubscribeChannelInfo(channel.Subscription, channel.Path)
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

        public PointToPointChannelInfo GetInfo(PointToPointChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    var queue = serviceBusNamespace.Queues.GetByName(channel.Path);

                    var info = new PointToPointChannelInfo(channel.Path)
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

        public bool CreateIfNotExist(PointToPointChannel channel)
        {
            var configuration = JsonConvert.DeserializeObject<AzureServiceBusConfiguration>(channel.ConnectionString);

            var serviceBusNamespace = GetServiceBusNamespace(configuration);

            if (serviceBusNamespace != null)
            {
                try
                {
                    serviceBusNamespace.Queues.GetByName(channel.Path);
                    
                    return false;
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var messagettl =14;

                        var lockduration = 300;

                        if(channel.Properties.ContainsKey("defaultmessagettlindays"))
                        {
                            messagettl = Convert.ToInt32(channel.Properties["defaultmessagettlindays"]);
                        }

                        if (channel.Properties.ContainsKey("messagelockdurationinseconds"))
                        {
                            lockduration = Convert.ToInt32(channel.Properties["messagelockdurationinseconds"]);
                        }

                        var descriptor = serviceBusNamespace.Queues.Define(channel.Path)
                            .WithDefaultMessageTTL(TimeSpan.FromDays(messagettl))
                            .WithMessageLockDurationInSeconds(lockduration);

                        if (channel.Properties.ContainsKey("duplicatemessagedetectioninminutes"))
                        {
                            var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties["duplicatemessagedetectioninminutes"]);

                            descriptor = descriptor.WithDuplicateMessageDetection(TimeSpan.FromMinutes(duplicatemessagedetectioninminutes));
                        }

                        if (channel.Properties.ContainsKey("sessionenabled"))
                        {
                            descriptor = descriptor.WithSession();
                        }

                        if (channel.Properties.ContainsKey("partitioningenabled"))
                        {
                            descriptor = descriptor.WithPartitioning();
                        }

                        if (channel.Properties.ContainsKey("expressmessageenabled"))
                        {
                            descriptor = descriptor.WithExpressMessage();
                        }


                        descriptor.Create();

                        return true;
                    }

                }
            }

            return false;
        }

        private IServiceBusNamespace GetServiceBusNamespace(AzureServiceBusConfiguration configuration)
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