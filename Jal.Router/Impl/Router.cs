using System;
using System.Diagnostics;
using System.Linq;
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
                var contenttype = typeof(TContent);

                var routes = Provider.Provide(contenttype, routename);

                if (routes != null && routes.Length > 0)
                {
                    _invoker.Invoke(context, routes);
                }
                else
                {
                    throw new ApplicationException($"No route to handle the Content {nameof(TContent)} and name {routename}");
                }

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
