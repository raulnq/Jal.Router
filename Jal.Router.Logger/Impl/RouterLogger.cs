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
                _log.Info($"[Router.cs, Route, {context.Data.Id}] Start Call. Message arrived. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");

                await next(context);

                _log.Info($"[Router.cs, Route, {context.Data.Id}] Message routed. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Data.Id}] Exception. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Router.cs, Route, {context.Data.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} route: {context.Data.Route?.Name} saga: {context.Data.Saga?.Name} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
        }
    }
}