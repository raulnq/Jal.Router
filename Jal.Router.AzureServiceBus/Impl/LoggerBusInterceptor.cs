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

        public override void OnSendEntry(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] Start Call. id: {context.Id} connectionstring: {context.ToConnectionString} path: {context.ToPath} replyconnectionstring: {context.ReplyToConnectionString} replypath: {context.ReplyToPath}");
        }

        public override void OnSendExit(OutboundMessageContext context,Options options, long duration)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] End Call. Took {duration} ms.");
        }

        public override void OnSendSuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Send, {options.Correlation}] Message sent. id: {context.Id} connectionstring: {context.ToConnectionString} path: {context.ToPath} replyconnectionstring: {context.ReplyToConnectionString} replypath: {context.ReplyToPath}");
        }

        public override void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Send, {options.Correlation}] Exception.", ex);
        }

        public override void OnReplyEntry(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Reply, {options.Correlation}] Start Call. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath} replyconnectionstring: {context.ReplyToConnectionString} replypath: {context.ReplyToPath}");
        }

        public override void OnReplyExit(OutboundMessageContext context, Options options, long duration)
        {
            _log.Info($"[Bus.cs, Reply, {options.Correlation}] End Call. Took {duration} ms.");
        }

        public override void OnReplySuccess(OutboundMessageContext context, Options options)
        {
            _log.Info($"[Bus.cs, Reply, {options.Correlation}] Message sent. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath} replyconnectionstring: {context.ReplyToConnectionString} replypath: {context.ReplyToPath}");
        }

        public override void OnReplyError(OutboundMessageContext context, Options options, Exception ex)
        {
            _log.Error($"[Bus.cs, Reply, {options.Correlation}] Exception.", ex);
        }
    }
}