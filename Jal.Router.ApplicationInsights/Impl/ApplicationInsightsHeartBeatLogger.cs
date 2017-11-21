using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsHeartBeatLogger : ILogger<HeartBeat>
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsHeartBeatLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(HeartBeat info, DateTime datetime)
        {
            _client.TrackMetric($"{info.Name} - Running", 1);
        }
    }
}