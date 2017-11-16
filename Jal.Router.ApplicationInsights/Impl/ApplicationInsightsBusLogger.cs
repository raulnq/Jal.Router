using System;
using System.Collections.Generic;
using Jal.Router.Impl.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbount;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsBusLogger : AbstractBusLogger
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsBusLogger(TelemetryClient client)
        {
            _client = client;
        }

        public override void OnSendExit(OutboundMessageContext context, Options options, long duration)
        {
            var dt = new DependencyTelemetry()
            {
                Name = context.ContentType.Name,
                Duration = TimeSpan.FromMilliseconds(duration),
                Success = true,
                Id = context.Id,
                Timestamp = context.DateTimeUtc,
                Target = context.ToPath,
                ResultCode = "200",
                Type = "PointToPoint",
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
            _client.TrackDependency(dt);  
        }

        public override void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
            _client.TrackException(ex);
        }

        public override void OnPublishExit(OutboundMessageContext context, Options options, long duration)
        {
            var dt = new DependencyTelemetry()
            {
                Name = context.ContentType.Name,
                Duration = TimeSpan.FromMilliseconds(duration),
                Success = true,
                Id = context.Id,
                Timestamp = context.DateTimeUtc,
                Target = context.ToPath,
                ResultCode = "200",
                Type = "PublishSubscriber",
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
            _client.TrackDependency(dt);
        }

        public override void OnPublishError(OutboundMessageContext context, Options options, Exception ex)
        {
            _client.TrackException(ex);
        }
    }
}