using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights
{
    public class SubscriptionToPublishSubscribeChannelStatisticsLogger : ILogger<SubscriptionToPublishSubscribeChannelStatistics>
    {
        private readonly TelemetryClient _client;
        public SubscriptionToPublishSubscribeChannelStatisticsLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(SubscriptionToPublishSubscribeChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _client.TrackMetric($"{statistics.Path}/{statistics.Subscription} - {item.Key}", Double.Parse(item.Value));
            }
        }
    }
}