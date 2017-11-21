using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.Interface.Inbound;
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

        public void Execute<TContent>(InboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Router.cs, Route, {context.Id}] Start Call. Message arrived. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");

                next();

                _log.Info($"[Router.cs, Route, {context.Id}] Message routed. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Router.cs, Route, {context.Id}] Exception.", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Router.cs, Route, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. Message routed. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
            }
        }
    }
}