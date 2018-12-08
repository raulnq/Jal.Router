using System;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class AbstractRouterConfigurationSourceExtensions
    {
        public static void RegisterQueue<TValueFinder>(this AbstractRouterConfigurationSource configuration, string queue, Func<IValueFinder, ServiceBusConfiguration> servicebusconfigurationprovider)
            where TValueFinder : IValueFinder
        {

            Func<IValueFinder, string> provider = finder =>
            {
                var servicebusconfiguration = servicebusconfigurationprovider(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            configuration.RegisterPointToPointChannel<TValueFinder>(queue, provider);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, string queue, ServiceBusConfiguration servicebusconfiguration)
        {
            configuration.RegisterPointToPointChannel(queue, JsonConvert.SerializeObject(servicebusconfiguration));
        }

        public static void RegisterTopic<TValueFinder>(this AbstractRouterConfigurationSource configuration, string topic,
            Func<IValueFinder, ServiceBusConfiguration> servicebusconfigurationprovider)
            where TValueFinder : IValueFinder
        {
            Func<IValueFinder, string> provider = finder =>
            {
                var servicebusconfiguration = servicebusconfigurationprovider(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            configuration.RegisterPublishSubscribeChannel<TValueFinder>(topic, provider);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, string topic, ServiceBusConfiguration servicebusconfiguration)
        {
            configuration.RegisterPublishSubscribeChannel(topic, JsonConvert.SerializeObject(servicebusconfiguration));
        }

        public static void RegisterSubscriptionToTopic<TValueFinder>(this AbstractRouterConfigurationSource configuration, string subscription, string topic,
            Func<IValueFinder, ServiceBusConfiguration> servicebusconfigurationprovider, string filter=null)
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

            configuration.RegisterSubscriptionToPublishSubscribeChannel<TValueFinder>(subscription, topic, provider, r);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, string subscription, string topic, ServiceBusConfiguration servicebusconfiguration, string filter=null)
        {

            SubscriptionToPublishSubscribeChannelRule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new SubscriptionToPublishSubscribeChannelRule() { Filter = filter, IsDefault = true, Name = "$Default" };
            }

            configuration.RegisterSubscriptionToPublishSubscribeChannel(subscription, topic, JsonConvert.SerializeObject(servicebusconfiguration), r);
        }
    }
}