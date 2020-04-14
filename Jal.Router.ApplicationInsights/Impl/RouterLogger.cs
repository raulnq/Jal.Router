using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{

    public class RouterLogger : AbstractApplicationInsightsLogger, IAsyncMiddleware<MessageContext>
    {

        public RouterLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }


        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var telemetry = new RequestTelemetry();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                telemetry.Timestamp = context.Data.DateTimeUtc;

                telemetry.Id = $"{context.Data.TracingContext.Id}";

                telemetry.Name = context.Data.Name;

                telemetry.Source = context.Data.Origin.From;
                
                PopulateProperties(telemetry.Properties, context.Data);

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