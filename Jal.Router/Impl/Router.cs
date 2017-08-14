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

        private readonly INoTypedRouter _notypedrouter;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public Router(IMessageAdapter<TMessage> adapter, IRouteProvider provider, INoTypedRouter notypedrouter)
        {
            _adapter = adapter;

            Provider = provider;

            _notypedrouter = notypedrouter;

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

                _notypedrouter.Route(context.Content, context, routes, null);

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

    public interface ISagaRouter<in TMessage>
    {
        void Route<TContent>(TMessage message, string saganame = "");
    }

    public class SagaRouter<TMessage> : ISagaRouter<TMessage>
    {
        public ISagaProvider Provider { get; set; }

        private readonly INoTypedRouter _notypedrouter;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public void Route<TContent>(TMessage message, string saganame = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var bodytype = typeof(TContent);

                //IsStart -> TContent + saganame

                var sagas = Provider.Provide(bodytype, saganame);

                foreach (var saga in sagas)
                {
                    var start = saga.Routes.Where(x => x.First && x.BodyType == typeof(TContent));

                    var @continue = saga.Routes.Where(x => !x.First && x.BodyType == typeof(TContent));

                    //if (first.BodyType == typeof (TContent))
                    //{
                    //    Start(context.Content, context, new [] { first });
                    //}
                    //else
                    //{


                    //    Continue(context.Content, context, new[] { first });
                    //}

                    
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

        public void Continue<TContent>(TContent content, InboundMessageContext context, Route[] routes)
        {
            //create data
            //_notypedrouter.Route(context.Content, context, saga.Routes.ToArray());
            //save data
        }

        public void Start<TContent>(TContent content, InboundMessageContext context, Route[] routes)
        {
            //get data
            //_notypedrouter.Route(context.Content, context, saga.Routes.ToArray());
            //save data
        }
    }
}
