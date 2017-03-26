using System;
using Common.Logging;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class LoggerBusInterceptor : AbstractBusInterceptor
    {
        private readonly ILog _log;

        public LoggerBusInterceptor(ILog log)
        {
            _log = log;
        }

        public override void OnEntry(OutboundMessageContext context, Options options, string method)
        {
            _log.Info($"[Bus.cs, {method}, {options.Correlation}] Start Call. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath}");
        }

        public override void OnExit(OutboundMessageContext context,Options options, string method, long duration)
        {
            _log.Info($"[Bus.cs, {method}, {options.Correlation}] End Call. Took {duration} ms.");
        }

        public override void OnSuccess(OutboundMessageContext context, Options options, string method)
        {
            _log.Info($"[Bus.cs, {context}, {options.Correlation}] Sending Message. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath}");
        }

        public override void OnError(OutboundMessageContext context, Options options, string method, Exception ex)
        {
            _log.Error($"[Bus.cs, {method}, {options.Correlation}] Exception.", ex);
        }
    }
}