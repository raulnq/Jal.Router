using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsPointToPointChannelInfoLogger : ILogger<PointToPointChannelInfo>
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsPointToPointChannelInfoLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(PointToPointChannelInfo info, DateTime datetime)
        {
            _client.TrackMetric($"{info.Name} - MessageCount", info.MessageCount);
            _client.TrackMetric($"{info.Name} - SizeInBytes", info.SizeInBytes);
            _client.TrackMetric($"{info.Name} - DeadLetterMessageCount", info.DeadLetterMessageCount);
            _client.TrackMetric($"{info.Name} - ScheduledMessageCount", info.ScheduledMessageCount);
        }
    }
}