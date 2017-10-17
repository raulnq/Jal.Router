using System;
using Common.Logging;
using Jal.Router.Impl.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Logger.Impl
{
    public class RouterLogger : AbstractRouterLogger
    {
        private readonly ILog _log;

        public RouterLogger(ILog log)
        {
            _log = log;
        }

        public override void OnEntry(MessageContext context)
        {
            _log.Info($"[Router.cs, Route, {context.Id}] Start Call. Message arrived. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
        }

        public override void OnSuccess<TContent>(MessageContext context, TContent content)
        {
            _log.Info( $"[Router.cs, Route, {context.Id}] Message routed. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
        }

        public override void OnExit(MessageContext context, long duration)
        {
            _log.Info($"[Router.cs, Route, {context.Id}] End Call. Took {duration} ms. Message routed. id: {context.Id} sagaid: {context.Saga?.Id} from: {context.Origin.Name} origin: {context.Origin.Key} retry: {context.RetryCount}");
        }

        public override void OnException(MessageContext context, Exception exception)
        {
            _log.Error($"[Router.cs, Route, {context.Id}] Exception.", exception);
        }
    }
}