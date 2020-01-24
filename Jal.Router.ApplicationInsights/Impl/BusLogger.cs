using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public class BusLogger : AbstractApplicationInsightsLogger, IMiddlewareAsync<MessageContext>
    {

        public BusLogger(TelemetryClient client, IConfiguration configuration):base(client, configuration)
        {

        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var telemetry = new DependencyTelemetry()
            {
                Name = context.Data.Name,

                Id = context.Data.IdentityContext.Id,

                Timestamp = context.Data.DateTimeUtc,

                Target = context.Data.Channel.FullPath,

                Data = context.Data.ContentContext.Data,

                Type = Configuration.TransportName,
            };

            PopulateContext(telemetry.Context, context.Data);

            PopulateProperties(telemetry.Properties, context.Data);

            try
            {
                await next(context);

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