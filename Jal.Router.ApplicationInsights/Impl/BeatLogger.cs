using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;
using Microsoft.ApplicationInsights;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class BeatLogger : ILogger<Beat>
    {
        private readonly TelemetryClient _client;
        public BeatLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Log(Beat message, DateTime datetime)
        {
            _client.TrackMetric($"{message.Name} - {message.Action}", 1);
        }
    }
}