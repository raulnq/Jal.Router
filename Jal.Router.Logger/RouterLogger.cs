using System;
using Common.Logging;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Logger
{
    public class RouterLogger : AbstractRouterLogger
    {
        private readonly ILog _log;

        public RouterLogger(ILog log)
        {
            _log = log;
        }

        public override void OnEntry(InboundMessageContext context)
        {
            _log.Info($"[Router.cs, Route, {context.Id}] Start Call. Message arrived. id: {context.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
        }

        public override void OnSuccess<TContent>(InboundMessageContext context, TContent content)
        {
            _log.Info( $"[Router.cs, Route, {context.Id}] Message routed. id: {context.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
        }

        public override void OnExit(InboundMessageContext context, long duration)
        {
            _log.Info($"[Router.cs, Route, {context.Id}] End Call. Took {duration} ms.");
        }

        public override void OnException(InboundMessageContext context, Exception exception)
        {
            _log.Error($"[Router.cs, Route, {context.Id}] Exception.", exception);
        }
    }
}