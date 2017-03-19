using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class Router : IRouter
    {
        public IConsumerFactory Factory { get; set; }

        public IRouteProvider Provider { get; set; }

        public IRouterInterceptor Interceptor { get; set; }

        public Router(IConsumerFactory factory, IRouteProvider provider)
        {
            Factory = factory;

            Provider = provider;

            Interceptor = AbstractRouterInterceptor.Instance;
        }

        public void InternalRoute<TBody, TConsumer>(TBody body, dynamic context, Route<TBody, TConsumer> route) where TConsumer : class
        {
            var consumer = Factory.Create<TConsumer>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                try
                {
                    Interceptor.OnEntry(body, consumer);

                    if (routeMethod.EvaluatorWithContext == null)
                    {
                        if (routeMethod.Evaluator == null)
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(body, consumer, context);

                                Interceptor.OnSuccess(body, consumer);
                            }
                            else
                            {
                                if (routeMethod.Consumer != null)
                                {
                                    routeMethod.Consumer(body, consumer);

                                    Interceptor.OnSuccess(body, consumer);
                                }
                            }
                        }
                        else
                        {
                            if (routeMethod.Evaluator(body, consumer))
                            {
                                if (routeMethod.ConsumerWithContext != null)
                                {
                                    routeMethod.ConsumerWithContext(body, consumer, context);

                                    Interceptor.OnSuccess(body, consumer);
                                }
                                else
                                {
                                    if (routeMethod.Consumer != null)
                                    {
                                        routeMethod.Consumer(body, consumer);

                                        Interceptor.OnSuccess(body, consumer);
                                    }
                                }
                            }
                        }

                        Interceptor.OnSuccess(body, consumer);
                    }
                    else
                    {
                        if (routeMethod.EvaluatorWithContext(body, consumer, context))
                        {
                            if (routeMethod.ConsumerWithContext != null)
                            {
                                routeMethod.ConsumerWithContext(body, consumer, context);

                                Interceptor.OnSuccess(body, consumer);
                            }
                            else
                            {
                                if (routeMethod.Consumer != null)
                                {
                                    routeMethod.Consumer(body, consumer);

                                    Interceptor.OnSuccess(body, consumer);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Interceptor.OnError(body, consumer, ex);
                    throw;
                }
                finally
                {
                    Interceptor.OnExit(body, consumer);
                }
            }
        }

        public void Route<TBody>(TBody body, string name = "")
        {
            var bodytype = typeof (TBody);

            var routes = Provider.Provide(bodytype, name);

            foreach (var route in routes)
            {
                var routemethod = typeof(Router).GetMethods().First(x => x.Name == "InternalRoute" && x.GetParameters().Count() == 2);

                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new[] { (object)body, route });
            }
        }

        public void Route<TBody>(TBody body, dynamic context, string name = "")
        {
            var bodytype = typeof(TBody);

            var routes = Provider.Provide(bodytype, name);

            foreach (var route in routes)
            {
                var routemethod = typeof(Router).GetMethods().First(x => x.Name == "InternalRoute" && x.GetParameters().Count() == 3);

                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new[] { (object)body, context, route });
            }
        }

        public void InternalRoute<TBody, TConsumer>(TBody body, Route<TBody, TConsumer> route) where TConsumer : class 
        {
            var consumer = Factory.Create<TConsumer>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                try
                {
                    Interceptor.OnEntry(body, consumer);

                    if (routeMethod.Evaluator == null)
                    {
                        if (routeMethod.Consumer != null)
                        {
                            routeMethod.Consumer(body, consumer);

                            Interceptor.OnSuccess(body, consumer);
                        }
                    }
                    else
                    {
                        if (routeMethod.Evaluator(body, consumer))
                        {
                            if (routeMethod.Consumer != null)
                            {
                                routeMethod.Consumer(body, consumer);

                                Interceptor.OnSuccess(body, consumer);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Interceptor.OnError(body, consumer, ex);
                    throw;
                }
                finally
                {
                    Interceptor.OnExit(body, consumer);
                }
            }
        }
    }
}
