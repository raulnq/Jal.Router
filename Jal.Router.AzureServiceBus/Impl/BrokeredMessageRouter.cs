using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;
using Jal.Router.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageRouter : IBrokeredMessageRouter
    {
        public IRouter Router { get; set; }

        public IBrokeredMessageReader Reader { get; set; }

        public IBrokeredMessageRouterInterceptor Interceptor { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageReader reader)
        {
            _log = log;

            Router = router;

            Reader = reader;

            Interceptor = AbstractRouterInterceptor.Instance;
        }

        public void Route<TContent>(BrokeredMessage brokeredMessage, string name="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = new BrokeredMessageContext();

            var messageid = brokeredMessage.MessageId;

            var contenttype = brokeredMessage.ContentType;

            var replyto = brokeredMessage.ReplyTo;

            var to = brokeredMessage.To;

            var correlationid = brokeredMessage.CorrelationId;

            var source = string.Empty;

            if (brokeredMessage.Properties.ContainsKey("source"))
            {
                source = brokeredMessage.Properties["source"].ToString();
            }

            _log.Info($"[BrokeredMessageRouter.cs, Route, {messageid}] Start Call. MessageId: {messageid} CorrelationId: {correlationid} Content: {contenttype} From: {source} To: {to} ReplyTo: {replyto}");

            var body = default(TContent); 

            try
            {
                body = Reader.Read<TContent>(brokeredMessage);

                Interceptor.OnEntry(body, brokeredMessage);

                Router.Route<TContent>(body, context, name);

                Interceptor.OnSuccess(body, brokeredMessage);
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageRouter.cs, Route, {messageid}] Exception.", ex);

                Interceptor.OnException(body, brokeredMessage, ex);

                throw;
            }
            finally
            {
                Interceptor.OnExit(body, brokeredMessage);

                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, Route, {messageid}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }
    }
}
