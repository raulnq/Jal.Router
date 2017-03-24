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

        public IBrokeredMessageEndPointSettingProvider SettingsProvider { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageAdapter adapter, IBrokeredMessageContextBuilder builder, IBrokeredMessageEndPointProvider provider, IBrokeredMessageEndPointSettingProvider settingsprovider)
        {
            _log = log;

            Router = router;

            Adapter = adapter;

            Interceptor = AbstractBrokeredMessageRouterInterceptor.Instance;

            Builder = builder;

            Provider = provider;

            SettingsProvider = settingsprovider;
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

                    _log.Info($"[BrokeredMessageRouter.cs, ReplyToQueue, {context.MessageId}] Sending message to connectionstring: {context.ReplyToConnectionString} queue: {context.ReplyToQueue} messageId: {message.MessageId} correlationid: {message.CorrelationId}");

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

        public void SendToQueue<TContent>(TContent content, BrokeredMessageContext context, BrokeredMessageEndPoint endpoint, string messageid)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] Start Call.");

            try
            {
                if (!string.IsNullOrWhiteSpace(endpoint.ToConnectionString) && !string.IsNullOrWhiteSpace(endpoint.To))
                {
                    var queueclient = QueueClient.CreateFromConnectionString(endpoint.ToConnectionString, endpoint.To);

                    var message = Adapter.Writer(content);


                    if (!string.IsNullOrWhiteSpace(endpoint.ReplyToConnectionString) && !string.IsNullOrWhiteSpace(endpoint.ReplyTo))
                    {
                        message.ReplyTo = $"{endpoint.ReplyToConnectionString};queue={endpoint.ReplyTo}";

                        message.Properties.Add("replytoconnectionstring", endpoint.ReplyToConnectionString);

                        message.Properties.Add("replytoqueue", endpoint.ReplyTo);
                    }

                    if (!string.IsNullOrWhiteSpace(messageid))
                    {
                        message.MessageId = messageid;
                    }

                    if (!string.IsNullOrWhiteSpace(endpoint.From))
                    {
                        message.Properties.Add("from", endpoint.From);
                    }

                    _log.Info($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] Sending message to connectionstring: {endpoint.ToConnectionString} queue: {endpoint.To} messageId: {message.MessageId}");

                    queueclient.Send(message);

                    queueclient.Close();
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
                    var brokeredmessageendpoint = SettingsProvider.Provide<TContent>(endpoint, content);

                    if (!string.IsNullOrWhiteSpace(brokeredmessageendpoint.ToConnectionString) && !string.IsNullOrWhiteSpace(brokeredmessageendpoint.To))
                    {
                        var queueclient = QueueClient.CreateFromConnectionString(brokeredmessageendpoint.ToConnectionString, brokeredmessageendpoint.To);

                        var message = Adapter.Writer(content);

                        if (!string.IsNullOrWhiteSpace(brokeredmessageendpoint.ReplyToConnectionString) && !string.IsNullOrWhiteSpace(brokeredmessageendpoint.ReplyTo))
                        {
                            message.ReplyTo = $"{brokeredmessageendpoint.ReplyToConnectionString};queue={brokeredmessageendpoint.ReplyTo}";

                            message.Properties.Add("replytoconnectionstring", brokeredmessageendpoint.ReplyToConnectionString);

                            message.Properties.Add("replytoqueue", brokeredmessageendpoint.ReplyTo);
                        }
 
                        if (!string.IsNullOrWhiteSpace(messageid))
                        {
                            message.MessageId = messageid;
                        }

                        if (!string.IsNullOrWhiteSpace(brokeredmessageendpoint.From))
                        {
                            message.Properties.Add("from", brokeredmessageendpoint.From);
                        }

                        _log.Info($"[BrokeredMessageRouter.cs, SendToQueue, {context.MessageId}] Sending message to connectionstring: {brokeredmessageendpoint.ToConnectionString} queue: {brokeredmessageendpoint.To} messageId: {message.MessageId}");

                        queueclient.Send(message);

                        queueclient.Close();
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
