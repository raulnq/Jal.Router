using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Serilog
{
    public class RouterLogger : IAsyncMiddleware<MessageContext>
    {
        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var log = Log.Logger;

            if (!string.IsNullOrWhiteSpace(context.Data.TracingContext.ParentId))
            {
                log = log.ForContext("ParentId", context.Data.TracingContext.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.TracingContext.OperationId))
            {
                log = log.ForContext("OperationId", context.Data.TracingContext.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.SagaContext?.Data?.Id))
            {
                log = log.ForContext("SagaId", context.Data.SagaContext?.Data?.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.From))
            {
                log = log.ForContext("From", context.Data.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.Key))
            {
                log = log.ForContext("Origin", context.Data.Origin.Key);
            }

            try
            {
                log.Debug("[Router, Route, {Id}] Start Call. Message arrived. route: {Route} saga: {Saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);

                await next(context).ConfigureAwait(false);

                log.ForContext("Type","message-consumer").Information("[Router, Route, {Id}] Message routed. route: {Route} saga: {Saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);
            }
            catch (Exception exception)
            {
                log.Error(exception, "[Router, Route, {Id}] Exception. route: {Route} saga: {Saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                log.Debug("[Router, Route, {Id}] End Call. Took {Duration} ms. Message routed. route: {Route} saga: {Saga}", context.Data.Id, stopwatch.ElapsedMilliseconds, context.Data.Route?.Name, context.Data.Saga?.Name);
            }
        }
    }
}