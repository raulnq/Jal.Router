using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class PointToPointChannelInfoLogger : ILogger<PointToPointChannelInfo>
    {
        private readonly TelemetryClient _client;
        public PointToPointChannelInfoLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(PointToPointChannelInfo info, DateTime datetime)
        {
            _client.TrackMetric($"{info.Path} - MessageCount", info.MessageCount);
            _client.TrackMetric($"{info.Path} - SizeInBytes", info.SizeInBytes);
            _client.TrackMetric($"{info.Path} - DeadLetterMessageCount", info.DeadLetterMessageCount);
            _client.TrackMetric($"{info.Path} - ScheduledMessageCount", info.ScheduledMessageCount);
        }
    }
}