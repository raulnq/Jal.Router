using System;
using System.Diagnostics;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Router<TMessage> : IRouter<TMessage>
    {
        public IRouteProvider Provider { get; set; }

        private readonly IRouterInvoker _invoker;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public Router(IMessageAdapter<TMessage> adapter, IRouteProvider provider, IRouterInvoker invoker)
        {
            _adapter = adapter;

            Provider = provider;

            _invoker = invoker;

            Interceptor = AbstractRouterInterceptor.Instance;

            Logger = AbstractRouterLogger.Instance;
        }

        public void Route<TContent>(TMessage message, string routename = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.ReadContext(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var contenttype = typeof(TContent);

                var routes = Provider.Provide(contenttype, routename);

                var content = _adapter.ReadContent<TContent>(message);

                if (routes != null && routes.Length > 0)
                {
                    _invoker.Invoke(new InboundMessageContext<TContent>(context, content), routes);

                    Logger.OnSuccess(context, content);

                    Interceptor.OnSuccess(context, content);
                }
                else
                {
                    throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and route name {routename}");
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException ?? ex;

                Logger.OnException(context, inner);

                Interceptor.OnException(context, inner);

                throw inner;
            }
            finally
            {
                stopwatch.Stop();

                Logger.OnExit(context, stopwatch.ElapsedMilliseconds);

                Interceptor.OnExit(context);
            }
        }
    }
}
