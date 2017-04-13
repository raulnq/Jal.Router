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
        }
    }

    public class Router<TMessage> : IRouter<TMessage>
    {
        public IHandlerFactory Factory { get; set; }

        public IRouteProvider Provider { get; set; }

        private readonly IValueSettingFinderFactory _finderFactory;

        private readonly IBus _bus;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public Router(IMessageAdapter<TMessage> adapter, IValueSettingFinderFactory finderFactory, IBus bus)
        {
            _adapter = adapter;

            _finderFactory = finderFactory;

            _bus = bus;

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
                Route(context.Content, context, name);

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

        public Router(IHandlerFactory factory, IRouteProvider provider, IValueSettingFinderFactory finderFactory, IBus bus)
        {
            Factory = factory;

            Provider = provider;

            _finderFactory = finderFactory;

            _bus = bus;
        }

        public void InternalRoute<TContent, THandler>(TContent content, InboundMessageContext context, Route<TContent, THandler> route) where THandler : class
        {
            var consumer = Factory.Create<THandler>(route.ConsumerType);

            foreach (var routeMethod in route.RouteMethods)
            {
                IRetryPolicy policy = null;

                try
                {
                    if (routeMethod.RetryExceptionType != null && routeMethod.RetryPolicyExtractor != null)
                    {
                        var extractor = _finderFactory.Create(routeMethod.RetryExtractorType);

                        policy = routeMethod.RetryPolicyExtractor(extractor);

                        if (policy != null)
                        {
                            var interval = policy.NextRetryInterval(context.RetryCount);

                            context.LastRetry = !policy.CanRetry(context.RetryCount, interval);
                        }
                    }


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
                catch (Exception ex)
                {
                    if (policy!=null)
                    {
                        if (routeMethod.RetryExceptionType == ex.GetType())
                        {
                            if (!context.LastRetry)
                            {
                                _bus.Retry(content, context, policy);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Route<TContent>(TContent content, InboundMessageContext context, string name = "")
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
    }
}
