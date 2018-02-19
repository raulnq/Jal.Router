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
                _log.Info($"[Router.cs, Route, {context.Id}] Start Call. Message arrived. id: {context.Id} sagaid: {context.SagaInfo?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name}");

                next();

                _log.Info($"[Router.cs, Route, {context.Id}] Message routed. id: {context.Id} sagaid: {context.SagaInfo?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Id}] Exception.", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Router.cs, Route, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Id} sagaid: {context.SagaInfo?.Id} from: {context.Origin.From} origin: {context.Origin.Key} retry: {context.RetryCount} route: {parameter.Route?.Name} saga: {parameter.Saga?.Name}");
            }
        }
    }
}