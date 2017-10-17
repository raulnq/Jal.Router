using System;
using Common.Logging;
using Jal.Router.Impl.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbount;

namespace Jal.Router.Logger.Impl
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
            _log.Info($"[Bus.cs, Send, {context.Id}] Start Call. id: {context.Id} sagaid: {context.Saga?.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin.Key}");
        }

        public override void OnSendExit(OutboundMessageContext context,Options options, long duration)
        {
            _log.Info($"[Bus.cs, Send, {context.Id}] End Call. Took {duration} ms.");
        }

        public override void OnSendSuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Send, {context.Id}] Message sent. id: {context.Id} sagaid: {context.Saga?.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin.Key}");
        }

        public override void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Send, {context.Id}] Exception.", ex);
        }

        public override void OnPublishEntry(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Publish, {context.Id}] Start Call. id: {context.Id} sagaid: {context.Saga?.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin.Key}");
        }

        public override void OnPublishExit(OutboundMessageContext context, Options options, long duration)
        {
            _log.Info($"[Bus.cs, Publish, {context.Id}] End Call. Took {duration} ms.");
        }

        public override void OnPublishSuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Publish, {context.Id}] Message sent. id: {context.Id} sagaid: {context.Saga?.Id} toconnectionstring: {context.ToConnectionString} topath: {context.ToPath} with origin: {context.Origin.Key}");
        }

        public override void OnPublishError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Publish, {context.Id}] Exception.", ex);
        }
    }
}