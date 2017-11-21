using System;
using System.Diagnostics;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Inbound;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsRouterLogger : IMiddleware
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsRouterLogger(TelemetryClient client)
        {
            _client = client;
        }
        public void Execute<TContent>(IndboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            var telemetry = new RequestTelemetry();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                telemetry.Timestamp = context.DateTimeUtc;
                telemetry.Id = context.Id;
                telemetry.Name = context.ContentType.Name;
                telemetry.Properties.Add("from", context.Origin.Name);
                telemetry.Properties.Add("version", context.Version);
                telemetry.Properties.Add("origin", context.Origin.Key);
                telemetry.Properties.Add("saga", context.Saga?.Id);
                telemetry.Context.Operation.Id = $"{context.Id}{context.RetryCount}";
                foreach (var h in context.Headers)
                {
                    telemetry.Properties.Add(h.Key, h.Value);
                }

                telemetry.Metrics.Add("retry", context.RetryCount);

                next();

                telemetry.ResponseCode = "200";
                telemetry.Success = true;
                    
            }
            catch (Exception exception)
            {
                telemetry.ResponseCode = "500";
                telemetry.Success = false;

                var telemetryexception = new ExceptionTelemetry(exception);
                telemetry.Context.Operation.Id = $"{context.Id}{context.RetryCount}";

                _client.TrackException(telemetryexception);
                throw;
            }
            finally
            {
                telemetry.Duration = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                _client.TrackRequest(telemetry);
            }
        }
    }
}