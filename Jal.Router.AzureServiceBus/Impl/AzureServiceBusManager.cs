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

        public bool CreateIfNotExistSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, string origin)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetSubscription(path, subscription);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var subscriptiondescription = new SubscriptionDescription(path, subscription)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),

                        LockDuration = TimeSpan.FromMinutes(5),
                    };

                    var ruledescriptor = new RuleDescription("Default", new SqlFilter($"origin='{origin}'"));

                    namespaceManager.CreateSubscription(subscriptiondescription, ruledescriptor);

                    return true;
                }
            }

            return false;
        }

        public bool CreateIfNotExistPublishSubscribeChannel(string connectionstring, string path)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetTopic(path);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var topicDescription = new TopicDescription(path)
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



        public PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string path)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetTopic(path);
                    var info = new PublishSubscribeChannelInfo(path)
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

        public SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetSubscription(path, subscription);
                    var info = new SubscriptionToPublishSubscribeChannelInfo(subscription, path)
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

        public PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string path)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    var entity = namespaceManager.GetQueue(path);

                    var info = new PointToPointChannelInfo(path)
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

        public bool CreateIfNotExistPointToPointChannel(string connectionstring, string path)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetQueue(path);

                    return false;
                }
                catch (MessagingEntityNotFoundException)
                {
                    var queueDescription = new QueueDescription(path)
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