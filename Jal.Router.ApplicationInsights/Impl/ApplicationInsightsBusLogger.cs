using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsBusLogger : IMiddleware
    {
        private readonly TelemetryClient _client;
        public ApplicationInsightsBusLogger(TelemetryClient client)
        {
            _client = client;
        }
        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var telemetry = new DependencyTelemetry()
            {
                Name = context.ContentType.Name,
                Id = context.Id,
                Timestamp = context.DateTimeUtc,
                Target = context.ToPath,
                Data = context.ContentAsString,
                Type = parameter.OutboundType == "Send" ? "Queue" : "Topic" ,
                Properties =
                {
                    new KeyValuePair<string, string>("from", context.Origin.Name),
                    new KeyValuePair<string, string>("origin", context.Origin.Key),
                    new KeyValuePair<string, string>("saga",context.SagaInfo?.Id),
                    new KeyValuePair<string, string>("version", context.Version),
                },
                Metrics =
                {
                    new KeyValuePair<string, double>("retry", context.RetryCount)
                }
            };
            
            telemetry.Context.Operation.Id = $"{context.Id}{context.RetryCount}";
            telemetry.Context.Operation.ParentId = $"{context.Id}{context.RetryCount}";
            foreach (var h in context.Headers)
            {
                telemetry.Properties.Add(h.Key, h.Value);
            }
            try
            {
                next();
                telemetry.Success = true;
                telemetry.ResultCode = "200";
            }
            catch (Exception exception)
            {
                telemetry.Success = false;
                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);
                telemetryexception.Context.Operation.Id = $"{context.Id}{context.RetryCount}";
                telemetryexception.Context.Operation.ParentId = $"{context.Id}{context.RetryCount}";
                _client.TrackException(telemetryexception);

                throw;
            }
            finally
            {
                telemetry.Duration = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                _client.TrackDependency(telemetry);
            }
        }
    }
}