using System;
using System.Collections.Generic;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class AbstractRouterConfigurationSourceExtensions
    {
        public static void RegisterQueue<TValueFinder>(this AbstractRouterConfigurationSource configuration, AzureServiceBusQueue queue, Func<IValueFinder, AzureServiceBusConfiguration> servicebusconfigurationprovider)
            where TValueFinder : IValueFinder
        {

            Func<IValueFinder, string> provider = finder =>
            {
                var servicebusconfiguration = servicebusconfigurationprovider(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

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

            configuration.RegisterPointToPointChannel<TValueFinder>(queue.Name, provider, dictionary);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, string queue, AzureServiceBusConfiguration servicebusconfiguration)
        {
            RegisterQueue(configuration, new AzureServiceBusQueue(queue), servicebusconfiguration);
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

        public static void RegisterTopic<TValueFinder>(this AbstractRouterConfigurationSource configuration, AzureServiceBusTopic topic,
            Func<IValueFinder, AzureServiceBusConfiguration> servicebusconfigurationprovider)
            where TValueFinder : IValueFinder
        {
            Func<IValueFinder, string> provider = finder =>
            {
                var servicebusconfiguration = servicebusconfigurationprovider(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

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

            configuration.RegisterPublishSubscribeChannel<TValueFinder>(topic.Name, provider, dictionary);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, string topic, AzureServiceBusConfiguration servicebusconfiguration)
        {
            RegisterTopic(configuration, new AzureServiceBusTopic(topic), servicebusconfiguration);
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

        public static void RegisterSubscriptionToTopic<TValueFinder>(this AbstractRouterConfigurationSource configuration, AzureServiceBusSubscriptionToTopic subscription,
            Func<IValueFinder, AzureServiceBusConfiguration> servicebusconfigurationprovider, string filter = null)
            where TValueFinder : IValueFinder
        {
            Func<IValueFinder, string> provider = finder =>
            {
                var servicebusconfiguration = servicebusconfigurationprovider(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            SubscriptionToPublishSubscribeChannelRule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new SubscriptionToPublishSubscribeChannelRule() { Filter = filter, IsDefault = true, Name = "$Default" };
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

            configuration.RegisterSubscriptionToPublishSubscribeChannel<TValueFinder>(subscription.Name, subscription.Topic, provider, dictionary, r);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, AzureServiceBusSubscriptionToTopic subscription, AzureServiceBusConfiguration servicebusconfiguration, string filter = null)
        {

            SubscriptionToPublishSubscribeChannelRule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new SubscriptionToPublishSubscribeChannelRule() { Filter = filter, IsDefault = true, Name = "$Default" };
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
    }
}