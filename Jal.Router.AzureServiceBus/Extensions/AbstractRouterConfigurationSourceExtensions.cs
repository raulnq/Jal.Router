using System;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Extensions
{
    public static class AbstractRouterConfigurationSourceExtensions
    {
        public static void RegisterQueue<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string name, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            configuration.RegisterPointToPointChannel<TExtractorConectionString>(name, connectionstringextractor);
        }

        public static void RegisterTopic<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string name,
            Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            configuration.RegisterPublishSubscriberChannel<TExtractorConectionString>(name, connectionstringextractor);
        }

        public static void RegisterSubscriptionToTopic<TExtractorConectionString>(this AbstractRouterConfigurationSource configuration, string name, string path,
            Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            configuration.RegisterSubscriptionToPublishSubscriberChannel<TExtractorConectionString>(name, path, connectionstringextractor);
        }
    }
}