using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SubscriptionToPublishSubscribeChannelStatisticsLogger : ILogger<SubscriptionToPublishSubscribeChannelStatistics>
    {
        private readonly ILogger _logger;
        public SubscriptionToPublishSubscribeChannelStatisticsLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(SubscriptionToPublishSubscribeChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _logger.Log($"{statistics.Path} {item.Key}:{item.Value}");
            }
        }
    }
}