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

        public IBrokeredMessageEndPointProvider Provider { get; set; }

        public IBrokeredMessageSettingsExtractorFactory Factory { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageAdapter adapter, IBrokeredMessageContextBuilder builder, IBrokeredMessageEndPointProvider provider, IBrokeredMessageSettingsExtractorFactory factory)
        {
            _log = log;

            Router = router;

            Adapter = adapter;

            Interceptor = AbstractBrokeredMessageRouterInterceptor.Instance;

            Builder = builder;

            Provider = provider;

            Factory = factory;
        }

        public void Route<TContent>(BrokeredMessage brokeredMessage, string name="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = Builder.Build(brokeredMessage);
           
            _log.Info($"[BrokeredMessageRouter.cs, Route, {context.MessageId}] Start Call. MessageId: {context.MessageId} CorrelationId: {context.CorrelationId} From: {context.From} To: {context.To}");

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

        public void ReplyToQueue<TContent>(TContent content, BrokeredMessageContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageRouter.cs, ReplyToQueue, {context.MessageId}] Start Call. ReplyToConnectionString: {context.ReplyToConnectionString} ReplyToQueue: {context.ReplyToQueue}");

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
                _log.Error($"[BrokeredMessageRouter.cs, ReplyToQueue, {context.MessageId}] Exception.", ex);
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, ReplyToQueue, {context.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }

        public void SendToQueue<TContent>(TContent content, BrokeredMessageContext context, string messageid, string name = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] Start Call.");

            try
            {
                var endpoints = Provider.Provide<TContent>(name);

                foreach (var endpoint in endpoints)
                {
                    var extractor = Factory.Create(endpoint.ExtractorType);

                    var toconnectionextractor = endpoint.ToConnectionStringExtractor as Func<IBrokeredMessageSettingsExtractor, string>;

                    var topathextractor = endpoint.ToPathExtractor as Func<IBrokeredMessageSettingsExtractor, string>;

                    if (toconnectionextractor != null && topathextractor != null)
                    {
                        var toconnection = toconnectionextractor(extractor);

                        var topath = topathextractor(extractor);

                        if (!string.IsNullOrWhiteSpace(toconnection) && !string.IsNullOrWhiteSpace(topath))
                        {
                            var queueclient = QueueClient.CreateFromConnectionString(toconnection, topath);

                            var message = Adapter.Writer(content);

                            var toreplyconnectionextractor = endpoint.ReplyToConnectionStringExtractor as Func<IBrokeredMessageSettingsExtractor, string>;

                            var toreplypathextractor = endpoint.ReplyToPathExtractor as Func<IBrokeredMessageSettingsExtractor, string>;

                            if (toreplyconnectionextractor != null && toreplypathextractor != null)
                            {
                                var toreplyconnection = toreplyconnectionextractor(extractor);

                                var toreplypath = toreplypathextractor(extractor);

                                if (!string.IsNullOrWhiteSpace(toreplyconnection) && !string.IsNullOrWhiteSpace(toreplypath))
                                {
                                    message.ReplyTo = $"{toreplyconnection};queue={toreplypath}";

                                    message.Properties.Add("replytoconnectionstring", toreplyconnection);

                                    message.Properties.Add("replytoqueue", toreplypath);
                                }
                            }

                            var fromextractor = endpoint.ReplyToPathExtractor as Func<IBrokeredMessageSettingsExtractor, string>;

                            var from = fromextractor?.Invoke(extractor);

                            if (!string.IsNullOrWhiteSpace(messageid))
                            {
                                message.MessageId = messageid;
                            }

                            if (!string.IsNullOrWhiteSpace(from))
                            {
                                message.Properties.Add("from", from);
                            }

                            queueclient.Send(message);

                            queueclient.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] Exception.", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }
    }
}
