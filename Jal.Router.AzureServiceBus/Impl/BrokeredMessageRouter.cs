using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageRouter : IBrokeredMessageRouter
    {
        public IRouter Router { get; set; }

        public IBrokeredMessageRouterInterceptor Interceptor { get; set; }

        private readonly IBrokeredMessageContentAdapter _contentAdapter;

        private readonly IBrokeredMessageFromAdapter _fromAdapter;

        private readonly IBrokeredMessageIdAdapter _idAdapter;

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageContentAdapter contentAdapter, IBrokeredMessageFromAdapter fromAdapter, IBrokeredMessageIdAdapter idAdapter)
        {
            _log = log;

            Router = router;

            _contentAdapter = contentAdapter;

            _fromAdapter = fromAdapter;

            _idAdapter = idAdapter;

            Interceptor = AbstractBrokeredMessageRouterInterceptor.Instance;
        }

        public void Route<TContent>(BrokeredMessage brokeredMessage, string name="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _log.Info($"[BrokeredMessageRouter.cs, Route, {brokeredMessage.MessageId}] Start Call.");

            Interceptor.OnEntry(brokeredMessage);

            try
            {
                var context = new InboundMessageContext
                {
                    Id = _idAdapter.Read(brokeredMessage),
                    From = _fromAdapter.Read(brokeredMessage),
                    Origin = _fromAdapter.ReadOrigin(brokeredMessage),
                };

                _log.Info($"[BrokeredMessageRouter.cs, Route, {context.Id}] Message arrived. id: {context.Id} from: {context.From} origin: {context.Origin}");

                var body = _contentAdapter.Read<TContent>(brokeredMessage);

                if (brokeredMessage.Properties != null)
                {
                    foreach (var property in brokeredMessage.Properties)
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                Router.Route(body, context, name);

                Interceptor.OnSuccess(body, brokeredMessage);
            }
            catch (Exception ex)
            {
                _log.Error($"[BrokeredMessageRouter.cs, Route, {brokeredMessage.MessageId}] Exception.", ex);

                Interceptor.OnException(brokeredMessage, ex);

                throw;
            }
            finally
            {
                Interceptor.OnExit(brokeredMessage);

                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, Route, {brokeredMessage.MessageId}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }
        }

        public string GetConnectionString(string connectionstringandqueuename)
        {
            if (string.IsNullOrEmpty(connectionstringandqueuename) || connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                return string.Empty;
            }

            return connectionstringandqueuename.Substring(0, connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetEntity(string connectionstringandqueuename)
        {
            if (!string.IsNullOrWhiteSpace(connectionstringandqueuename))
            {
                if (string.IsNullOrEmpty(connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7)))
                {
                    return string.Empty;
                }

                return connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7);
            }

            return string.Empty;
        }
    }
}
