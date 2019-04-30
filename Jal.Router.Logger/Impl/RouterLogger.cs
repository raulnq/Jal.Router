using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Logging;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Logger.Impl
{
    public class RouterLogger : IMiddlewareAsync<MessageContext>
    {
        private readonly ILog _log;

        public RouterLogger(ILog log)
        {
            _log = log;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Router.cs, Route, {context.Data.IdentityContext.Id}] Start Call. Message arrived. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} retry: {context.Data.RetryCount} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");

                await next(context);

                _log.Info($"[Router.cs, Route, {context.Data.IdentityContext.Id}] Message routed. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} retry: {context.Data.RetryCount} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Data.IdentityContext.Id}] Exception. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} retry: {context.Data.RetryCount} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Router.cs, Route, {context.Data.IdentityContext.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} retry: {context.Data.RetryCount} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
        }
    }
}