using System;
using System.Diagnostics;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsRouterLogger : IMiddleware
    {
        private readonly TelemetryClient _client;

        private readonly IConfiguration _configuration;

        public ApplicationInsightsRouterLogger(TelemetryClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var telemetry = new RequestTelemetry();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var name = parameter.Route.Name;

            if (!string.IsNullOrWhiteSpace(parameter.Saga?.Name))
            {
                name = $"{parameter.Saga?.Name}_{name}";
            }

            try
            {
                telemetry.Timestamp = context.DateTimeUtc;
                telemetry.Id = context.Id;
                telemetry.Name = name;
                telemetry.Properties.Add("from", context.Origin.From);
                telemetry.Properties.Add("version", context.Version);
                telemetry.Properties.Add("origin", context.Origin.Key);
                telemetry.Properties.Add("sagaid", context.SagaContext?.Id);
                telemetry.Properties.Add("replytorequestid", context.ReplyToRequestId);
                telemetry.Properties.Add("requestid", context.RequestId);

                telemetry.Context.Operation.Id = $"{context.Id}{context.RetryCount}";
                telemetry.Context.Operation.Name = name;
                telemetry.Context.Operation.ParentId = $"{context.Id}{context.RetryCount}";
                telemetry.Source = context.Origin.From;
                if (!string.IsNullOrWhiteSpace(_configuration.ApplicationName))
                {
                    telemetry.Context.Cloud.RoleName = _configuration.ApplicationName;
                }

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
                telemetryexception.Context.Operation.Id = $"{context.Id}{context.RetryCount}";
                telemetryexception.Context.Operation.ParentId = $"{context.Id}{context.RetryCount}";
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