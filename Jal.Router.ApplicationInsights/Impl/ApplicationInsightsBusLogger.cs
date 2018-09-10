using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class ApplicationInsightsBusLogger : AbstractApplicationInsightsLogger, IMiddleware
    {

        public ApplicationInsightsBusLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }
        public object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var telemetry = new DependencyTelemetry()
            {
                Name = context.EndPoint.Name,

                Id = context.Identity.Id,

                Timestamp = context.DateTimeUtc,

                Target = middlewarecontext.Channel.GetPath(),

                Data = context.Content,

                Type = Configuration.ChannelProviderName,
            };

            PopulateContext(telemetry.Context, context);

            PopulateProperties(telemetry.Properties, context);

            PopulateMetrics(telemetry.Metrics, context);


            try
            {
                var result = next(context, middlewarecontext);

                telemetry.Success = true;

                telemetry.ResultCode = "200";

                return result;
            }
            catch (Exception exception)
            {
                telemetry.Success = false;

                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);

                PopulateContext(telemetryexception.Context, context);

                PopulateProperties(telemetryexception.Properties, context);

                PopulateMetrics(telemetryexception.Metrics, context);

                Client.TrackException(telemetryexception);

                throw;
            }
            finally
            {
                telemetry.Duration = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

                Client.TrackDependency(telemetry);
            }
        }
    }
}