using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Logger.Impl
{
    public class RouterLogger : IMiddleware
    {
        private readonly ILog _log;

        public RouterLogger(ILog log)
        {
            _log = log;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Router.cs, Route, {context.Identity.Id}] Start Call. Message arrived. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");

                next();

                _log.Info($"[Router.cs, Route, {context.Identity.Id}] Message routed. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Identity.Id}] Exception. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Router.cs, Route, {context.Identity.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");
            }
        }
    }
}