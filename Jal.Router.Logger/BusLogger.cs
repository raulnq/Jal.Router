using System;
using Common.Logging;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Logger
{
    public class BusLogger : AbstractBusLogger
    {
        private readonly ILog _log;

        public BusLogger(ILog log)
        {
            _log = log;
        }

        public override void OnSendEntry(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] Start Call. id: {context.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin}");
        }

        public override void OnSendExit(OutboundMessageContext context,Options options, long duration)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] End Call. Took {duration} ms.");
        }

        public override void OnSendSuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] Message sent. id: {context.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin}");
        }

        public override void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Send, {options.Correlation}] Exception.", ex);
        }

        public override void OnPublishEntry(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Publish, {options.Correlation}] Start Call. id: {context.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin}");
        }

        public override void OnPublishExit(OutboundMessageContext context, Options options, long duration)
        {
            _log.Info($"[Bus.cs, Publish, {options.Correlation}] End Call. Took {duration} ms.");
        }

        public override void OnPublishSuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Publish, {options.Correlation}] Message sent. id: {context.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin}");
        }

        public override void OnPublishError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Publish, {options.Correlation}] Exception.", ex);
        }
    }
}