using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsStartupBeatLogger : ILogger<StartupBeat>
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsStartupBeatLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(StartupBeat info, DateTime datetime)
        {
            _client.TrackMetric($"{info.Name} - Started", 1);
        }
    }
}