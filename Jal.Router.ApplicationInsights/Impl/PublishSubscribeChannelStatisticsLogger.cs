using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class PublishSubscribeChannelStatisticsLogger : ILogger<PublishSubscribeChannelStatistics>
    {
        private readonly TelemetryClient _client;
        public PublishSubscribeChannelStatisticsLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(PublishSubscribeChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _client.TrackMetric($"{statistics.Path} - {item.Key}", Double.Parse(item.Value));
            }
        }
    }
}