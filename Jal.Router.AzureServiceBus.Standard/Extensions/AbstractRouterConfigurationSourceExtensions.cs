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
        public static void RegisterQueue<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string queue, Func<IValueFinder, ServiceBusConfiguration> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {

            Func<IValueFinder, string> extractor = finder =>
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
            Func<IValueFinder, ServiceBusConfiguration> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            Func<IValueFinder, string> extractor = finder =>
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
            Func<IValueFinder, ServiceBusConfiguration> connectionstringextractor, string filter=null)
            where TExtractorConectionString : IValueFinder
        {
            Func<IValueFinder, string> extractor = finder =>
            {
                var servicebusconfiguration = connectionstringextractor(finder);

                return JsonConvert.SerializeObject(servicebusconfiguration);
            };

            SubscriptionToPublishSubscribeChannelRule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new SubscriptionToPublishSubscribeChannelRule() { Filter = filter, IsDefault = true, Name = "$Default" };
            }

            configuration.RegisterSubscriptionToPublishSubscriberChannel<TExtractorConectionString>(subscription, topic, extractor, r);
        }

        public static void RegisterSubscriptionToTopic(this AbstractRouterConfigurationSource configuration, string subscription, string topic, ServiceBusConfiguration servicebusconfiguration, string filter=null)
        {

            SubscriptionToPublishSubscribeChannelRule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new SubscriptionToPublishSubscribeChannelRule() { Filter = filter, IsDefault = true, Name = "$Default" };
            }

            configuration.RegisterSubscriptionToPublishSubscriberChannel(subscription, topic, JsonConvert.SerializeObject(servicebusconfiguration), r);
        }
    }
}