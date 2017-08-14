using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NoTypedRouter : INoTypedRouter
    {
        private readonly IValueSettingFinderFactory _finderFactory;

        private readonly IBus _bus;

        private readonly IHandlerFactory _factory;

        public NoTypedRouter(IValueSettingFinderFactory finderFactory, IBus bus, IHandlerFactory factory)
        {
            _finderFactory = finderFactory;
            _bus = bus;
            _factory = factory;
        }

        public void Route(object content, InboundMessageContext context, Route[] routes, object data)
        {
            foreach (var route in routes)
            {
                var routemethod = GetType().GetMethods().First(x => x.Name == nameof(TypedRoute) && x.GetParameters().Count() == 4);

                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new object[] { content, context, route, data });
            }
        }

        public void TypedRoute<TContent, THandler>(TContent content, InboundMessageContext context, Route<TContent, THandler> route, object data) where THandler : class
        {
            if (IsApplicable(content, context, route))
            {
                var consumer = _factory.Create<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    IRetryPolicy policy = null;

                    try
                    {
                        if (HasRetry(method))
                        {
                            policy = GetRetryPolicy(method);

                            if (policy != null)
                            {
                                var interval = policy.NextRetryInterval(context.RetryCount);

                                context.LastRetry = !policy.CanRetry(context.RetryCount, interval);
                            }
                        }


                        EvaluateAndConsume(content, context, method, consumer, data);
                    }
                    catch (Exception ex)
                    {
                        if (policy != null)
                        {
                            if (method.RetryExceptionType == ex.GetType())
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
        }

        private void EvaluateAndConsume<TContent, THandler>(TContent content, InboundMessageContext context, RouteMethod<TContent, THandler> routeMethod, THandler consumer, object data) where THandler : class
        {
            if (routeMethod.EvaluatorWithContext == null)
            {
                if (routeMethod.Evaluator == null)
                {
                    Consume(content, context, routeMethod, consumer, data);
                }
                else
                {
                    if (routeMethod.Evaluator(content, consumer))
                    {
                        Consume(content, context, routeMethod, consumer, data);
                    }
                }
            }
            else
            {
                if (routeMethod.EvaluatorWithContext(content, consumer, context))
                {
                    Consume(content, context, routeMethod, consumer, data);
                }
            }
        }

        private void Consume<TContent, THandler>(TContent content, InboundMessageContext context, RouteMethod<TContent, THandler> routeMethod,
            THandler consumer, object data) where THandler : class
        {
            if (routeMethod.ConsumerWithContext != null)
            {
                routeMethod.ConsumerWithContext(content, consumer, context);
            }
            else
            {
                if (routeMethod.Consumer != null)
                {
                    routeMethod.Consumer(content, consumer);
                }
                else
                {
                    if (routeMethod.ConsumerWithDataAndContext != null)
                    {
                        routeMethod.ConsumerWithDataAndContext(content, consumer, context, data);
                    }
                    else
                    {
                        routeMethod.ConsumerWithData?.Invoke(content, consumer, data);
                    }
                }
            }
        }

        private IRetryPolicy GetRetryPolicy<TContent, THandler>(RouteMethod<TContent, THandler> routeMethod)
            where THandler : class
        {
            var extractor = _finderFactory.Create(routeMethod.RetryExtractorType);

            return routeMethod.RetryPolicyExtractor(extractor);
        }

        private bool HasRetry<TContent, THandler>(RouteMethod<TContent, THandler> routeMethod) where THandler : class
        {
            return routeMethod.RetryExceptionType != null && routeMethod.RetryPolicyExtractor != null;
        }

        private bool IsApplicable<TContent, THandler>(TContent content, InboundMessageContext context, Route<TContent, THandler> route)
            where THandler : class
        {
            var when = true;

            if (route.When != null)
            {
                when = route.When(content, context);
            }
            return when;
        }
    }
}