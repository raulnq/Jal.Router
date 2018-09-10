using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{

    public class ApplicationInsightsRouterLogger : AbstractApplicationInsightsLogger, IMiddleware
    {

        public ApplicationInsightsRouterLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }



        public void Execute(MessageContext context, Action<MessageContext, MiddlewareContext> next, MiddlewareContext middlewarecontext)
        {
            var telemetry = new RequestTelemetry();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var name = context.Route.Name;

            if (!string.IsNullOrWhiteSpace(context.Saga?.Name))
            {
                name = $"{context.Saga?.Name}_{name}";
            }

            try
            {
                telemetry.Timestamp = context.DateTimeUtc;

                telemetry.Id = $"{context.Identity.Id}";

                telemetry.Name = name;

                telemetry.Source = context.Origin.From;

                PopulateProperties(telemetry.Properties, context);

                PopulateMetrics(telemetry.Metrics, context);

                PopulateContext(telemetry.Context, context);

                next(context, middlewarecontext);

                telemetry.ResponseCode = "200";

                telemetry.Success = true;
            }
            catch (Exception exception)
            {
                telemetry.ResponseCode = "500";

                telemetry.Success = false;

                var telemetryexception = new ExceptionTelemetry(exception);

                PopulateProperties(telemetryexception.Properties, context);

                PopulateMetrics(telemetryexception.Metrics, context);

                PopulateContext(telemetryexception.Context, context);

                Client.TrackException(telemetryexception);

                throw;
            }
            finally
            {
                telemetry.Duration = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

                Client.TrackRequest(telemetry);
            }
        }
    }
}