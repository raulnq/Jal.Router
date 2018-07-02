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

                var manager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

                foreach (var source in _sources)
                {
                    var subscriptions = source.GetSubscriptionsToPublishSubscribeChannel();

                    foreach (var subscription in subscriptions)
                    {
                        var info = manager.GetSubscriptionToPublishSubscribeChannel(subscription.ConnectionString, subscription.Path, subscription.Subscription);

                        if (info != null)
                        {
                            Array.ForEach(loggers, x => x.Log(info, datetime));
                        }
                    }
                }
            }
        }
    }
}