using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageBus : IBus
    {
        public IEndPointProvider Provider { get; set; }

        public IBrokeredMessageAdapter Adapter { get; set; }

        private readonly ILog _log;

        public BrokeredMessageBus(ILog log, IBrokeredMessageAdapter adapter, IEndPointProvider provider)
        {
            _log = log;

            Adapter = adapter;

            Provider = provider;
        }

        public void ReplyTo<TContent>(TContent content, Context context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageBus.cs, ReplyTo, {context.Id}] Start Call. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath}");

            try
            {
                if (!string.IsNullOrEmpty(context.ReplyToConnectionString) && !string.IsNullOrEmpty(context.ReplyToPath))
                {

                    var queueclient = QueueClient.CreateFromConnectionString(context.ReplyToConnectionString, context.ReplyToPath);

                    var message = Adapter.Writer(content);

                    message.CorrelationId = context.Id;

                    message.MessageId = context.Id;

                    _log.Info($"[BrokeredMessageBus.cs, ReplyTo, {context.Id}] Sending Message. id: {context.Id} connectionstring: {context.ReplyToConnectionString} path: {context.ReplyToPath}");

                    queueclient.Send(message);

                    queueclient.Close();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageBus.cs, ReplyTo, {context.Id}] Exception.", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageBus.cs, ReplyTo, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }

        public void Send<TContent>(TContent content, Context context, EndPointSetting endpoint, string id="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageBus.cs, Send, {context.Id}] Start Call. id: {id} connectionstring: {endpoint.ToConnectionString} path: {endpoint.ToPath}");

            try
            {
                if (!string.IsNullOrWhiteSpace(endpoint.ToConnectionString) && !string.IsNullOrWhiteSpace(endpoint.ToPath))
                {
                    var queueclient = QueueClient.CreateFromConnectionString(endpoint.ToConnectionString, endpoint.ToPath);

                    var message = Adapter.Writer(content);

                    if (!string.IsNullOrWhiteSpace(endpoint.ReplyToConnectionString) && !string.IsNullOrWhiteSpace(endpoint.ReplyToPath))
                    {
                        message.ReplyTo = $"{endpoint.ReplyToConnectionString};queue={endpoint.ReplyToPath}";//TODO delete

                        message.Properties.Add("replytoconnectionstring", endpoint.ReplyToConnectionString);

                        message.Properties.Add("replytoqueue", endpoint.ReplyToPath);
                    }

                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        message.MessageId = id;
                    }

                    if (!string.IsNullOrWhiteSpace(endpoint.From))
                    {
                        message.Properties.Add("from", endpoint.From);
                    }

                    _log.Info($"[BrokeredMessageBus.cs, Send, {context.Id}] Sending message to connectionstring: {endpoint.ToConnectionString} queue: {endpoint.ToPath} messageId: {message.MessageId}");

                    queueclient.Send(message);

                    queueclient.Close();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageBus.cs, Send, {context.Id}] Exception.", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageBus.cs, Send, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }

        public void Send<TContent>(TContent content, Context context, string id="", string name = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageBus.cs, Send, {context.Id}] Start Call.");

            try
            {
                var endpoints = Provider.Provide<TContent>(name);

                foreach (var endpoint in endpoints)
                {
                    var setting = Provider.Provide<TContent>(endpoint, content);

                    Send(content, context, setting, id);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageBus.cs, Send, {context.Id}] Exception.", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[BrokeredMessageBus.cs, Send, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }

        }
    }
}