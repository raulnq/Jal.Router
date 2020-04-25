using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights
{
    public class StatisticsLogger : ILogger<Statistic>
    {
        private readonly TelemetryClient _client;
        public StatisticsLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(Statistic statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _client.TrackMetric($"{statistics.Path}/{statistics.Subscription} - {item.Key}", Double.Parse(item.Value));
            }
        }
    }
}