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
                Name = context.Data.EndPoint.Name,

                Id = context.Data.IdentityContext.Id,

                Timestamp = context.Data.DateTimeUtc,

                Target = context.Data.Channel.GetPath(),

                Data = context.Data.Content,

                Type = Configuration.ChannelProviderName,
            };

            PopulateContext(telemetry.Context, context.Data);

            PopulateProperties(telemetry.Properties, context.Data);

            PopulateMetrics(telemetry.Metrics, context.Data);


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

                PopulateMetrics(telemetryexception.Metrics, context.Data);

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