using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Logging;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Logger.Impl
{
    public class RouterLogger : IAsyncMiddleware<MessageContext>
    {
        private readonly ILog _log;

        public RouterLogger(ILog log)
        {
            _log = log;
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Debug($"[Router.cs, Route, {context.Data.Id}] Start Call. Message arrived. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");

                await next(context);

                _log.Info($"[Router.cs, Route, {context.Data.Id}] Message routed. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Data.Id}] Exception. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Debug($"[Router.cs, Route, {context.Data.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");
            }
        }
    }
}