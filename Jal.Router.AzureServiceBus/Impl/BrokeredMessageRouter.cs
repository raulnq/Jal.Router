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

        public IBrokeredMessageAdapter Adapter { get; set; }

        public IContextBuilder Builder { get; set; }

        public IBrokeredMessageRouterInterceptor Interceptor { get; set; }

        private readonly ILog _log;

        public BrokeredMessageRouter(ILog log, IRouter router, IBrokeredMessageAdapter adapter, IContextBuilder builder)
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
           
            _log.Info($"[BrokeredMessageRouter.cs, Route, {context.Id}] Start Call. id: {context.Id} correlation: {context.Correlation} from: {context.From}");

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
                _log.Error($"[BrokeredMessageRouter.cs, Route, {context.Id}] Exception.", ex);

                Interceptor.OnException(body, brokeredMessage, ex);

                throw;
            }
            finally
            {
                Interceptor.OnExit(body, brokeredMessage);

                stopwatch.Stop();

                _log.Info($"[BrokeredMessageRouter.cs, Route, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms.");
            }
        }


    }
}
