using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RetryExecutor : IRetryExecutor
    {
        private readonly IValueSettingFinderFactory _finderFactory;

        private readonly IBus _bus;

        public RetryExecutor(IValueSettingFinderFactory finderFactory, IBus bus)
        {
            _finderFactory = finderFactory;
            _bus = bus;
        }

        private IRetryPolicy GetRetryPolicy<TContent, THandler>(RouteMethod<TContent, THandler> routeMethod) where THandler : class
        {
            var extractor = _finderFactory.Create(routeMethod.RetryExtractorType);

            return routeMethod.RetryPolicyExtractor(extractor);
        }
        private bool HasRetry<TContent, THandler>(RouteMethod<TContent, THandler> routeMethod) where THandler : class
        {
            return routeMethod.RetryExceptionType != null && routeMethod.RetryPolicyExtractor != null;
        }
        public void Execute<TContent, THandler>(Action action, RouteMethod<TContent, THandler> routeMethod, InboundMessageContext<TContent> context) where THandler : class
        {
            IRetryPolicy policy = null;

            try
            {
                if (HasRetry(routeMethod))
                {
                    policy = GetRetryPolicy(routeMethod);

                    if (policy != null)
                    {
                        var interval = policy.NextRetryInterval(context.RetryCount);

                        context.LastRetry = !policy.CanRetry(context.RetryCount, interval);
                    }
                }

                action();
            }
            catch (Exception ex)
            {
                if (policy != null)
                {
                    if (routeMethod.RetryExceptionType == ex.GetType())
                    {
                        if (!context.LastRetry)
                        {
                            _bus.Retry(context.Content, context, policy);
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