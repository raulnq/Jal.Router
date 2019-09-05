using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Serilog.Impl
{
    public class RouterLogger : IMiddlewareAsync<MessageContext>
    {
        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var log = Log.Logger;

            if (!string.IsNullOrWhiteSpace(context.Data.IdentityContext.ParentId))
            {
                log = log.ForContext("parentid", context.Data.IdentityContext.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.IdentityContext.OperationId))
            {
                log = log.ForContext("operationid", context.Data.IdentityContext.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.SagaContext?.SagaData?.Id))
            {
                log = log.ForContext("sagaid", context.Data.SagaContext?.SagaData?.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.From))
            {
                log = log.ForContext("from", context.Data.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.Key))
            {
                log = log.ForContext("origin", context.Data.Origin.Key);
            }

            try
            {
                log.Information("[Router.cs, Route, {id}] Start Call. Message arrived. route: {route} saga: {saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);

                await next(context);

                log.Information("[Router.cs, Route, {id}] Message routed. route: {route} saga: {saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);
            }
            catch (Exception exception)
            {
                log.Error(exception, "[Router.cs, Route, {id}] Exception. route: {route} saga: {saga}", context.Data.Id, context.Data.Route?.Name, context.Data.Saga?.Name);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                log.Information("[Router.cs, Route, {id}] End Call. Took {duration} ms. Message routed. route: {route} saga: {saga}", context.Data.Id, stopwatch.ElapsedMilliseconds, context.Data.Route?.Name, context.Data.Saga?.Name);
            }
        }
    }
}