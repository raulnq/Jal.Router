using System;
using System.Diagnostics;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class Router : IRouter
    {
        public IHandlerFactory Factory { get; set; }

        public IRouteProvider Provider { get; set; }

        public Router(IHandlerFactory factory, IRouteProvider provider)
        {
            Factory = factory;

            Provider = provider;
        }

        public void InternalRoute<TContent, THandler>(TContent content, dynamic context, Route<TContent, THandler> route) where THandler : class
        {
            var consumer = Factory.Create<THandler>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                try
                {
                    if (routeMethod.EvaluatorWithContext == null)
                    {
                        if (routeMethod.Evaluator == null)
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(content, consumer, context);
                            }
                            else
                            {
                                routeMethod.Consumer?.Invoke(content, consumer);
                            }
                        }
                        else
                        {
                            if (routeMethod.Evaluator(content, consumer))
                            {
                                if (routeMethod.ConsumerWithContext != null)
                                {
                                    routeMethod.ConsumerWithContext(content, consumer, context);
                                }
                                else
                                {
                                    routeMethod.Consumer?.Invoke(content, consumer);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (routeMethod.EvaluatorWithContext(content, consumer, context))
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(content, consumer, context);
                            }
                            else
                            {
                                routeMethod.Consumer?.Invoke(content, consumer);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {

                }
            }
        }

        public void Route<TContent>(TContent content, string name = "")
        {
            var bodytype = typeof (TContent);

            var routes = Provider.Provide(bodytype, name);

            foreach (var route in routes)
            {
                var routemethod = typeof(Router).GetMethods().First(x => x.Name == "InternalRoute" && x.GetParameters().Count() == 2);

                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new[] { (object)content, route });
            }
        }

        public void Route<TContent>(TContent content, dynamic context, string name = "")
        {
            var bodytype = typeof(TContent);

            var routes = Provider.Provide(bodytype, name);

            foreach (var route in routes)
            {
                var routemethod = typeof(Router).GetMethods().First(x => x.Name == "InternalRoute" && x.GetParameters().Count() == 3);

                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new object[] { content, context, route });
            }
        }

        public void InternalRoute<TContent, THandler>(TContent content, Route<TContent, THandler> route) where THandler : class 
        {
            var consumer = Factory.Create<THandler>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                try
                {
                    if (routeMethod.Evaluator == null)
                    {
                        routeMethod.Consumer?.Invoke(content, consumer);
                    }
                    else
                    {
                        if (routeMethod.Evaluator(content, consumer))
                        {
                            routeMethod.Consumer?.Invoke(content, consumer);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                }
            }
        }
    }

    public class Router<TMessage> : IRouter<TMessage>
    {
        private readonly IRouter _route;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public Router(IRouter router, IMessageAdapter<TMessage> adapter)
        {
            _route = router;

            _adapter = adapter;

            Interceptor = AbstractRouterInterceptor.Instance;

            Logger = AbstractRouterLogger.Instance;
        }

        public void Route<TContent>(TMessage message, string name = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                _route.Route(context.Content, context, name);

                Logger.OnSuccess(context, context.Content);

                Interceptor.OnSuccess(context, context.Content);
            }
            catch (Exception ex)
            {
                Logger.OnException(context, ex);

                Interceptor.OnException(context, ex);

                throw;
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
