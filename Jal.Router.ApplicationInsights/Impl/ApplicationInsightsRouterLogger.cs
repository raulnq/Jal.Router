using System;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.Logger.Impl
{
    public class ApplicationInsightsRouterLogger : AbstractRouterLogger
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsRouterLogger(TelemetryClient client)
        {
            _client = client;
        }

        public override void OnEntry(MessageContext context)
        {
            var e = new EventTelemetry()
            {
                Name = context.ContentType.Name,
                Properties =
                {
                    new KeyValuePair<string, string>("origin", context.Origin.Name),
                    new KeyValuePair<string, string>("key", context.Origin.Key),
                    new KeyValuePair<string, string>("saga",context.Saga?.Id)
                },
                Metrics =
                {
                    new KeyValuePair<string, double>("retry", context.RetryCount)
                }
            };
            _client.TrackEvent(e);
        }

        public override void OnExit(MessageContext context, long duration)
        {

            var rt = new RequestTelemetry()
            {
                Name = context.ContentType.Name,
                Duration = TimeSpan.FromMilliseconds(duration),
                ResponseCode = "200",
                Success = true,
                Id = context.Id,
                Timestamp = context.DateTimeUtc,
                Properties =
                {
                    new KeyValuePair<string, string>("origin", context.Origin.Name),
                    new KeyValuePair<string, string>("key", context.Origin.Key),
                    new KeyValuePair<string, string>("saga",context.Saga?.Id)
                },
                Metrics =
                {
                    new KeyValuePair<string, double>("retry", context.RetryCount) 
                }
            };

            _client.TrackRequest(rt);
        }

        public override void OnException(MessageContext context, Exception exception)
        {
            _client.TrackException(exception);
        }
    }
}