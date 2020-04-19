using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights
{
    public class BusLogger : AbstractApplicationInsightsLogger, IAsyncMiddleware<MessageContext>
    {

        public BusLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var telemetry = new DependencyTelemetry()
            {
                Name = context.Data.Name,

                Id = context.Data.TracingContext.Id,

                Timestamp = context.Data.DateTimeUtc,

                Target = context.Data.Channel.FullPath,

                Data = context.Data.ContentContext.Data,

                Type = Configuration.TransportName,
            };

            PopulateContext(telemetry.Context, context.Data);

            PopulateProperties(telemetry.Properties, context.Data);

            try
            {
                await next(context).ConfigureAwait(false);

                telemetry.Success = true;

                telemetry.ResultCode = "200";
            }
            catch (Exception exception)
            {
                telemetry.Success = false;

                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);

                PopulateContext(telemetryexception.Context, context.Data);

                PopulateProperties(telemetryexception.Properties, context.Data);

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