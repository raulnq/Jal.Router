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

        public IBrokeredMessageAdapter Adapter { get; set; }

        public IBrokeredMessageContextBuilder Builder { get; set; }

        public IBrokeredMessageRouterInterceptor Interceptor { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageAdapter adapter, IBrokeredMessageContextBuilder builder)
        {
            _log = log;

            Router = router;

            Adapter = adapter;

            Interceptor = AbstractBrokeredMessageRouterInterceptor.Instance;

            Builder = builder;
        }

        public void Route<TContent>(BrokeredMessage brokeredMessage, string name="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = Builder.Build(brokeredMessage);
           
            _log.Info($"[BrokeredMessageRouter.cs, Route, {context.MessageId}] Start Call. MessageId: {context.MessageId} CorrelationId: {context.CorrelationId} From: {context.From} To: {context.To} ReplyTo: {context.ReplyToConnectionString}");

            var body = default(TContent); 

            try
            {
                body = Adapter.Read<TContent>(brokeredMessage);

                Interceptor.OnEntry(body, brokeredMessage);

                Router.Route(body, context, name);

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

        public void Reply<TContent>(TContent content, BrokeredMessageContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageRouter.cs, Reply, {context.MessageId}] Start Call.");

            try
            {
                if (!string.IsNullOrEmpty(context.ReplyToConnectionString) && !string.IsNullOrEmpty(context.ReplyToQueue))
                {
                    var queueclient = QueueClient.CreateFromConnectionString(context.ReplyToConnectionString, context.ReplyToQueue);

                    var message = Adapter.Writer(content);

                    message.CorrelationId = context.MessageId;

                    message.MessageId = context.MessageId;

                    queueclient.Send(message);

                    queueclient.Close();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageRouter.cs, Reply, {context.MessageId}] Exception.", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, Reply, {context.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }

        //public void Send<TContent>(TContent content, string name="")
        //{
        //    var stopwatch = new Stopwatch();

        //    stopwatch.Start();

        //    _log.Info($"[BrokeredMessageRouter.cs, Send, {context.MessageId}] Start Call.");

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(context.ReplyToConnectionString) && !string.IsNullOrEmpty(context.ReplyToQueue))
        //        {
        //            var queueclient = QueueClient.CreateFromConnectionString(context.ReplyToConnectionString, context.ReplyToQueue);

        //            var message = Adapter.Writer(content);

        //            message.CorrelationId = context.MessageId;

        //            message.MessageId = context.MessageId;

        //            queueclient.Send(message);

        //            queueclient.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error($"[BrokeredMessageRouter.cs, Send, {context.MessageId}] Exception.", ex);

        //        throw;
        //    }
        //    finally
        //    {
        //        stopwatch.Stop();

        //        _log.Info($"[BrokeredMessageRouter.cs, Send, {context.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
        //    }

        //}
    }
}
