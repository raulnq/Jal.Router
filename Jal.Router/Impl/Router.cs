using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class Router : IRouter
    {
        public IHandlerFactory Factory { get; set; }

        public IRouteProvider Provider { get; set; }

        public IRouterInterceptor Interceptor { get; set; }

        public Router(IHandlerFactory factory, IRouteProvider provider)
        {
            Factory = factory;

            Provider = provider;

            Interceptor = AbstractRouterInterceptor.Instance;
        }

        public void InternalRoute<TContent, THandler>(TContent content, dynamic context, Route<TContent, THandler> route) where THandler : class
        {
            var consumer = Factory.Create<THandler>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                try
                {
                    Interceptor.OnEntry(content, consumer);

                    if (routeMethod.EvaluatorWithContext == null)
                    {
                        if (routeMethod.Evaluator == null)
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(content, consumer, context);

                                Interceptor.OnSuccess(content, consumer);
                            }
                            else
                            {
                                if (routeMethod.Consumer != null)
                                {
                                    routeMethod.Consumer(content, consumer);

                                    Interceptor.OnSuccess(content, consumer);
                                }
                            }
                        }
                        else
                        {
                            if (routeMethod.Evaluator(content, consumer))
                            {
                                if (routeMethod.ConsumerWithContext != null)
                                {
                                    routeMethod.ConsumerWithContext(content, consumer, context);

                                    Interceptor.OnSuccess(content, consumer);
                                }
                                else
                                {
                                    if (routeMethod.Consumer != null)
                                    {
                                        routeMethod.Consumer(content, consumer);

                                        Interceptor.OnSuccess(content, consumer);
                                    }
                                }
                            }
                        }

                        Interceptor.OnSuccess(content, consumer);
                    }
                    else
                    {
                        if (routeMethod.EvaluatorWithContext(content, consumer, context))
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(content, consumer, context);

                                Interceptor.OnSuccess(content, consumer);
                            }
                            else
                            {
                                if (routeMethod.Consumer != null)
                                {
                                    routeMethod.Consumer(content, consumer);

                                    Interceptor.OnSuccess(content, consumer);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Interceptor.OnError(content, consumer, ex);
                    throw;
                }
                finally
                {
                    Interceptor.OnExit(content, consumer);
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
                    Interceptor.OnEntry(content, consumer);

                    if (routeMethod.Evaluator == null)
                    {
                        if (routeMethod.Consumer != null)
                        {
                            routeMethod.Consumer(content, consumer);

                            Interceptor.OnSuccess(content, consumer);
                        }
                    }
                    else
                    {
                        if (routeMethod.Evaluator(content, consumer))
                        {
                            if (routeMethod.Consumer != null)
                            {
                                routeMethod.Consumer(content, consumer);

                                Interceptor.OnSuccess(content, consumer);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Interceptor.OnError(content, consumer, ex);
                    throw;
                }
                finally
                {
                    Interceptor.OnExit(content, consumer);
                }
            }
        }
    }
}
