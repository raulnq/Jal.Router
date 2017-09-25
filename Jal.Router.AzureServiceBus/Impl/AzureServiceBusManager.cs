using System;
using System.Linq;
using Jal.Router.Interface;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusManager : IManager
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

        public void CreateSubscription(string connectionstring, string topicpath, string name, string origin)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetSubscription(topicpath, name);
                }
                catch (MessagingEntityNotFoundException)
                {
                    var subscriptiondescription = new SubscriptionDescription(topicpath, name)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),
                        LockDuration = TimeSpan.FromMinutes(5),
                    };
                    var ruledescriptor = new RuleDescription("Default", new SqlFilter($"origin='{origin}'"));
                    namespaceManager.CreateSubscription(subscriptiondescription, ruledescriptor);
                }
            }
        }

        public void CreateTopic(string connectionstring, string name)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetTopic(name);
                }
                catch (MessagingEntityNotFoundException)
                {
                    var topicDescription = new TopicDescription(name)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),
                        SupportOrdering = true
                    };
                    namespaceManager.CreateTopic(topicDescription);
                }
            }
        }

        public void CreateQueue(string connectionstring, string name)
        {
            var namespaceManager = GetNamespaceManager(connectionstring);

            if (namespaceManager != null)
            {
                try
                {
                    namespaceManager.GetQueue(name);
                }
                catch (MessagingEntityNotFoundException)
                {
                    var queueDescription = new QueueDescription(name)
                    {
                        DefaultMessageTimeToLive = TimeSpan.FromDays(14),
                        LockDuration = TimeSpan.FromMinutes(5),
                        SupportOrdering = true
                    };
                    namespaceManager.CreateQueue(queueDescription);
                }
            }
        }
    }
}