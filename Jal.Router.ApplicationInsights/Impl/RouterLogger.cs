using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{

    public class RouterLogger : AbstractApplicationInsightsLogger, IMiddlewareAsync<MessageContext>
    {

        public RouterLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }


        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var telemetry = new RequestTelemetry();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var name = context.Data.Route.Name;

            if (!string.IsNullOrWhiteSpace(context.Data.Saga?.Name))
            {
                name = $"{context.Data.Saga?.Name}_{name}";
            }

            try
            {
                telemetry.Timestamp = context.Data.DateTimeUtc;

                telemetry.Id = $"{context.Data.IdentityContext.Id}";

                telemetry.Name = name;

                telemetry.Source = context.Data.Origin.From;

                PopulateProperties(telemetry.Properties, context.Data);

                PopulateMetrics(telemetry.Metrics, context.Data);

                PopulateContext(telemetry.Context, context.Data);

                await next(context);

                telemetry.ResponseCode = "200";

                telemetry.Success = true;
            }
            catch (Exception exception)
            {
                telemetry.ResponseCode = "500";

                telemetry.Success = false;

                var telemetryexception = new ExceptionTelemetry(exception);

                PopulateProperties(telemetryexception.Properties, context.Data);

                PopulateMetrics(telemetryexception.Metrics, context.Data);

                PopulateContext(telemetryexception.Context, context.Data);

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