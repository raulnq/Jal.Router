using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class SubscriptionToPublishSubscribeChannelMonitor : IMonitoringTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public SubscriptionToPublishSubscribeChannelMonitor(IRouterConfigurationSource[] sources, IComponentFactory factory, IConfiguration configuration)
        {
            _sources = sources;
            _factory = factory;
            _configuration = configuration;
        }

        public void Run(DateTime datetime)
        {
            if (_configuration.LoggerTypes.ContainsKey(typeof(SubscriptionToPublishSubscribeChannelInfo)))
            {
                var loggertypes = _configuration.LoggerTypes[typeof(SubscriptionToPublishSubscribeChannelInfo)];

                var loggers = loggertypes.Select(x => _factory.Create<ILogger<SubscriptionToPublishSubscribeChannelInfo>>(x)).ToArray();

                var channelmanager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

                foreach (var source in _sources)
                {
                    var subscriptions = source.GetSubscriptionsToPublishSubscribeChannel();

                    foreach (var subscription in subscriptions)
                    {
                        var info = GetSubscriptionToPublishSubscribeChannel(subscription, channelmanager);

                        if (info != null)
                        {
                            Array.ForEach(loggers, x => x.Log(info, datetime));
                        }
                    }
                }
            }
        }

        public SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(SubscriptionToPublishSubscribeChannel subscriptionToPublishSubscribeChannel, IChannelManager channelmanager)
        {
            if (subscriptionToPublishSubscribeChannel.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(subscriptionToPublishSubscribeChannel.ConnectionStringExtractorType);

                var toconnectionextractor = subscriptionToPublishSubscribeChannel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null && !string.IsNullOrWhiteSpace(subscriptionToPublishSubscribeChannel.TopicPath))
                {
                    return channelmanager.GetSubscriptionToPublishSubscribeChannel(toconnectionextractor(extractorconnectionstring), subscriptionToPublishSubscribeChannel.TopicPath, subscriptionToPublishSubscribeChannel.Name);
                }
            }
            return null;
        }
    }
}