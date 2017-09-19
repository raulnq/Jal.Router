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

        private IRetryPolicy GetRetryPolicy<TContent, THandler>(RouteMethod<TContent, THandler> routemethod) where THandler : class
        {
            var extractor = _finderFactory.Create(routemethod.RetryExtractorType);

            return routemethod.RetryPolicyExtractor(extractor);
        }
        private bool HasRetry<TContent, THandler>(RouteMethod<TContent, THandler> routemethod) where THandler : class
        {
            return routemethod.RetryExceptionType != null && routemethod.RetryPolicyExtractor != null;
        }
        public void Execute<TContent, THandler>(Action action, RouteMethod<TContent, THandler> routemethod, InboundMessageContext<TContent> context) where THandler : class
        {
            IRetryPolicy policy = null;

            try
            {
                if (HasRetry(routemethod))
                {
                    policy = GetRetryPolicy(routemethod);

                    if (policy != null)
                    {
                        var interval = policy.NextRetryInterval(context.RetryCount+1);

                        context.LastRetry = !policy.CanRetry(context.RetryCount+1, interval);
                    }
                    else
                    {
                        throw new ApplicationException("The retry policy cannot be null");
                    }
                }

                action();
            }
            catch (Exception ex)
            {
                if (policy != null)
                {
                    if (routemethod.RetryExceptionType == ex.GetType())
                    {
                        if (!context.LastRetry)
                        {
                            _bus.Retry(context, policy);
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