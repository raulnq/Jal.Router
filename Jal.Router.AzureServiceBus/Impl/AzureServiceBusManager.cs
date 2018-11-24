using System;
using System.Linq;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusManager : IChannelManager
    {
        private const string ConnectionStringEndpoint = "endpoint";
        private const string ConnectionStringSharedAccessKeyName = "sharedaccesskeyname";
        private const string ConnectionStringSharedAccessKey = "sharedaccesskey";

        public NamespaceManager GetNamespaceManager(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                return null;
            }

            var toLower = connectionstring.ToLower();
            var parameters = connectionstring.Split(';')
                .ToDictionary(s => s.Substring(0, s.IndexOf('=')).ToLower(), s => s.Substring(s.IndexOf('=') + 1));

            if (toLower.Contains(ConnectionStringEndpoint) &&
                toLower.Contains(ConnectionStringSharedAccessKeyName) &&
                toLower.Contains(ConnectionStringSharedAccessKey))
            {
                if (parameters.Count < 3)
                {
                    return null;
                }
                var endpoint = parameters.ContainsKey(ConnectionStringEndpoint)
                    ? parameters[ConnectionStringEndpoint]
                    : null;

                if (string.IsNullOrWhiteSpace(endpoint))
                {
                    return null;
                }


                Uri uri;
                try
                {
                    uri = new Uri(endpoint);
                }
                catch (Exception)
                {
                    return null;
                }
                var ns = uri.Host.Split('.')[0];

                if (!parameters.ContainsKey(ConnectionStringSharedAccessKeyName) ||
                    string.IsNullOrWhiteSpace(parameters[ConnectionStringSharedAccessKeyName]))
                {
                    return null;
                }
                var sharedAccessKeyName = parameters[ConnectionStringSharedAccessKeyName];

                if (!parameters.ContainsKey(ConnectionStringSharedAccessKey) ||
                    string.IsNullOrWhiteSpace(parameters[ConnectionStringSharedAccessKey]))
                {
                    return null;
                }
                var sharedAccessKey = parameters[ConnectionStringSharedAccessKey];

                var sburi = ServiceBusEnvironment.CreateServiceUri("sb", ns, string.Empty);

                var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessKeyName, sharedAccessKey);

                return new NamespaceManager(sburi, tokenProvider);
            }

            return null;
        }

        public bool CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel)
        {
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetSubscription(channel.Path, channel.Subscription);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var subscriptiondescription = new SubscriptionDescription(channel.Path, channel.Subscription)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),

                        LockDuration = TimeSpan.FromMinutes(5),
                    };

                    var rule = channel.Rules.FirstOrDefault();

                    if (rule!=null)
                    {
                        var ruledescriptor = new RuleDescription(rule.Name, new SqlFilter(rule.Filter));

                        namespaceManager.CreateSubscription(subscriptiondescription, ruledescriptor);
                    }
                    else
                    {
                        namespaceManager.CreateSubscription(subscriptiondescription);
                    }


                    return true;
                }
            }

            return false;
        }

        public bool CreateIfNotExist(PublishSubscribeChannel channel)
        {
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetTopic(channel.Path);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var topicDescription = new TopicDescription(channel.Path)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),

                        SupportOrdering = true
                    };

                    namespaceManager.CreateTopic(topicDescription);

                    return true;
                }
            }

            return false;
        }



        public PublishSubscribeChannelInfo GetInfo(PublishSubscribeChannel channel)
        {
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetTopic(channel.Path);
                    var info = new PublishSubscribeChannelInfo(channel.Path)
                    {
                        MessageCount = entity.MessageCountDetails.ActiveMessageCount,
                        DeadLetterMessageCount = entity.MessageCountDetails.DeadLetterMessageCount,
                        ScheduledMessageCount = entity.MessageCountDetails.ScheduledMessageCount,
                        SizeInBytes = entity.SizeInBytes
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
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetSubscription(channel.Path, channel.Subscription);
                    var info = new SubscriptionToPublishSubscribeChannelInfo(channel.Subscription, channel.Path)
                    {
                        DeadLetterMessageCount = entity.MessageCountDetails.DeadLetterMessageCount,
                        MessageCount = entity.MessageCountDetails.ActiveMessageCount,
                        ScheduledMessageCount = entity.MessageCountDetails.ScheduledMessageCount
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
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetQueue(channel.Path);

                    var info = new PointToPointChannelInfo(channel.Path)
                    {
                        DeadLetterMessageCount = entity.MessageCountDetails.DeadLetterMessageCount,
                        MessageCount = entity.MessageCountDetails.ActiveMessageCount,
                        ScheduledMessageCount = entity.MessageCountDetails.ScheduledMessageCount,
                        SizeInBytes = entity.SizeInBytes
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
            var namespaceManager = GetNamespaceManager(channel.ConnectionString);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetQueue(channel.Path);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var queueDescription = new QueueDescription(channel.Path)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),
                        LockDuration = TimeSpan.FromMinutes(5),
                        SupportOrdering = true
                    };
                    namespaceManager.CreateQueue(queueDescription);

                    return true;
                }
            }

            return false;
        }
    }
}