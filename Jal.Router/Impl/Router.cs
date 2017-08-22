using System;
using System.Diagnostics;
using Jal.Router.Interface;

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

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var bodytype = typeof(TContent);

                var routes = Provider.Provide(bodytype, routename);

                _invoker.Invoke(context, routes);

                Logger.OnSuccess(context, context.Content);

                Interceptor.OnSuccess(context, context.Content);
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
