using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageRouter : IBrokeredMessageRouter
    {
        public IRouter Router { get; set; }

        public IBrokeredMessageReader Reader { get; set; }

        public IBrokeredMessageContextBuilder Builder { get; set; }

        public IBrokeredMessageRouterInterceptor Interceptor { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageReader reader, IBrokeredMessageContextBuilder builder)
        {
            _log = log;

            Router = router;

            Reader = reader;

            Interceptor = AbstractBrokeredMessageRouterInterceptor.Instance;

            Builder = builder;
        }

        public void Route<TContent>(BrokeredMessage brokeredMessage, string name="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = Builder.Build(brokeredMessage);
           
            _log.Info($"[BrokeredMessageRouter.cs, Route, {context.MessageId}] Start Call. MessageId: {context.MessageId} CorrelationId: {context.CorrelationId} From: {context.From} To: {context.To} ReplyTo: {context.ReplyTo}");

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
                _log.Error($"[BrokeredMessageRouter.cs, Route, {context.MessageId}] Exception.", ex);

                Interceptor.OnException(body, brokeredMessage, ex);

                throw;
            }
            finally
            {
                Interceptor.OnExit(body, brokeredMessage);

                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, Route, {context.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }
    }
}
