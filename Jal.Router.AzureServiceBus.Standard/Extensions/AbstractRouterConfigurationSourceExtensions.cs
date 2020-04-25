using System.Collections.Generic;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Model;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class AbstractRouterConfigurationSourceExtensions
    {
        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, string queue, AzureServiceBusConfiguration servicebusconfiguration)
        {
            RegisterQueue(configuration, new AzureServiceBusQueue(queue), servicebusconfiguration);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, string queue, string connectionstring)
        {
            RegisterQueue(configuration, new AzureServiceBusQueue(queue), connectionstring);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, AzureServiceBusQueue queue, AzureServiceBusConfiguration servicebusconfiguration)
        {
            var dictionary = new Dictionary<string, string>();

            if (queue.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", queue.DefaultMessageTtlInDays.ToString());
            }
            if (queue.DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add("duplicatemessagedetectioninminutes", queue.DuplicateMessageDetectionInMinutes.ToString());
            }
            if (queue.ExpressMessageEnabled != null)
            {
                dictionary.Add("expressmessageenabled", "true");
            }
            if (queue.MessageLockDurationInSeconds > 0)
            {
                dictionary.Add("messagelockdurationinseconds", queue.MessageLockDurationInSeconds.ToString());
            }
            if (queue.PartitioningEnabled != null)
            {
                dictionary.Add("partitioningenabled", "true");
            }
            if (queue.SessionEnabled != null)
            {
                dictionary.Add("sessionenabled", "true");
            }

            configuration.RegisterPointToPointChannel(queue.Name, JsonConvert.SerializeObject(servicebusconfiguration), dictionary);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, AzureServiceBusQueue queue, string connectionstring)
        {
            var dictionary = new Dictionary<string, string>();

            if (queue.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", queue.DefaultMessageTtlInDays.ToString());
            }
            if (queue.DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add("duplicatemessagedetectioninminutes", queue.DuplicateMessageDetectionInMinutes.ToString());
            }
            if (queue.ExpressMessageEnabled != null)
            {
                dictionary.Add("expressmessageenabled", "true");
            }
            if (queue.MessageLockDurationInSeconds > 0)
            {
                dictionary.Add("messagelockdurationinseconds", queue.MessageLockDurationInSeconds.ToString());
            }
            if (queue.PartitioningEnabled != null)
            {
                dictionary.Add("partitioningenabled", "true");
            }
            if (queue.SessionEnabled != null)
            {
                dictionary.Add("sessionenabled", "true");
            }

            configuration.RegisterPointToPointChannel(queue.Name, connectionstring, dictionary);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, string topic, AzureServiceBusConfiguration servicebusconfiguration)
        {
            RegisterTopic(configuration, new AzureServiceBusTopic(topic), servicebusconfiguration);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, string topic, string connectionstring)
        {
            RegisterTopic(configuration, new AzureServiceBusTopic(topic), connectionstring);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, AzureServiceBusTopic topic, AzureServiceBusConfiguration servicebusconfiguration)
        {
            var dictionary = new Dictionary<string, string>();

            if (topic.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", topic.DefaultMessageTtlInDays.ToString());
            }
            if (topic.DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add("duplicatemessagedetectioninminutes", topic.DuplicateMessageDetectionInMinutes.ToString());
            }
            if (topic.ExpressMessageEnabled != null)
            {
                dictionary.Add("expressmessageenabled", "true");
            }
            if (topic.PartitioningEnabled != null)
            {
                dictionary.Add("partitioningenabled", "true");
            }

            configuration.RegisterPublishSubscribeChannel(topic.Name, JsonConvert.SerializeObject(servicebusconfiguration), dictionary);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, AzureServiceBusTopic topic, string connectionstring)
        {
            var dictionary = new Dictionary<string, string>();

            if (topic.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", topic.DefaultMessageTtlInDays.ToString());
            }
            if (topic.DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add("duplicatemessagedetectioninminutes", topic.DuplicateMessageDetectionInMinutes.ToString());
            }
            if (topic.ExpressMessageEnabled != null)
            {
                dictionary.Add("expressmessageenabled", "true");
            }
            if (topic.PartitioningEnabled != null)
            {
                dictionary.Add("partitioningenabled", "true");
            }

            configuration.RegisterPublishSubscribeChannel(topic.Name, connectionstring, dictionary);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, AzureServiceBusSubscriptionToTopic subscription, AzureServiceBusConfiguration servicebusconfiguration, string filter = null)
        {

            Rule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new Rule(filter, "$Default", true);
            }

            var dictionary = new Dictionary<string, string>();

            if (subscription.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", subscription.DefaultMessageTtlInDays.ToString());
            }
            if (subscription.MessageLockDurationInSeconds > 0)
            {
                dictionary.Add("messagelockdurationinseconds", subscription.MessageLockDurationInSeconds.ToString());
            }
            if (subscription.SessionEnabled != null)
            {
                dictionary.Add("sessionenabled", "true");
            }

            configuration.RegisterSubscriptionToPublishSubscribeChannel(subscription.Name, subscription.Topic, JsonConvert.SerializeObject(servicebusconfiguration), dictionary, r);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, string subscription, string topic, AzureServiceBusConfiguration servicebusconfiguration, string filter = null)
        {
            RegisterSubscriptionToTopic(configuration, new AzureServiceBusSubscriptionToTopic(subscription, topic), servicebusconfiguration, filter);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, string subscription, string topic, string connectionstring, string filter = null)
        {
            RegisterSubscriptionToTopic(configuration, new AzureServiceBusSubscriptionToTopic(subscription, topic), connectionstring, filter);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, AzureServiceBusSubscriptionToTopic subscription, string connectionstring, string filter = null)
        {

            Rule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new Rule(filter, "$Default", true);
            }

            var dictionary = new Dictionary<string, string>();

            if (subscription.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", subscription.DefaultMessageTtlInDays.ToString());
            }
            if (subscription.MessageLockDurationInSeconds > 0)
            {
                dictionary.Add("messagelockdurationinseconds", subscription.MessageLockDurationInSeconds.ToString());
            }
            if (subscription.SessionEnabled != null)
            {
                dictionary.Add("sessionenabled", "true");
            }

            configuration.RegisterSubscriptionToPublishSubscribeChannel(subscription.Name, subscription.Topic, connectionstring, dictionary, r);
        }
    }
}