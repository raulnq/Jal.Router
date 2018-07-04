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
        public static void RegisterQueue<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string queue, Func<IValueSettingFinder, ServiceBusConfiguration> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {

            Func<IValueSettingFinder, string> extractor = finder =>
            {
                var servicebusconfiguration = connectionstringextractor(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            configuration.RegisterPointToPointChannel<TExtractorConectionString>(queue, extractor);
        }

        public static void RegisterQueue(this AbstractRouterConfigurationSource configuration, string queue, ServiceBusConfiguration servicebusconfiguration)
        {
            configuration.RegisterPointToPointChannel(queue, JsonConvert.SerializeObject(servicebusconfiguration));
        }

        public static void RegisterTopic<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string topic,
            Func<IValueSettingFinder, ServiceBusConfiguration> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            Func<IValueSettingFinder, string> extractor = finder =>
            {
                var servicebusconfiguration = connectionstringextractor(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            configuration.RegisterPublishSubscriberChannel<TExtractorConectionString>(topic, extractor);
        }

        public static void RegisterTopic(this AbstractRouterConfigurationSource configuration, string topic, ServiceBusConfiguration servicebusconfiguration)
        {
            configuration.RegisterPublishSubscriberChannel(topic, JsonConvert.SerializeObject(servicebusconfiguration));
        }

        public static void RegisterSubscriptionToTopic<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string subscription, string topic,
            Func<IValueSettingFinder, ServiceBusConfiguration> connectionstringextractor, SubscriptionToPublishSubscribeChannelRule rule=null)
            where TExtractorConectionString : IValueSettingFinder
        {
            Func<IValueSettingFinder, string> extractor = finder =>
            {
                var servicebusconfiguration = connectionstringextractor(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            configuration.RegisterSubscriptionToPublishSubscriberChannel<TExtractorConectionString>(subscription, topic, extractor, rule);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, string subscription, string topic, ServiceBusConfiguration servicebusconfiguration, SubscriptionToPublishSubscribeChannelRule rule=null)
        {
            configuration.RegisterSubscriptionToPublishSubscriberChannel(subscription, topic, JsonConvert.SerializeObject(servicebusconfiguration), rule);
        }
    }
}