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
                            if (!string.IsNullOrWhiteSpace(routemethod.OnRetryEndPoint))
                            {
                                SendRetry(routemethod, context, policy);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(routemethod.OnErrorEndPoint))
                                {
                                    SendError(routemethod, context);
                                }
                                else
                                {
                                    throw;
                                }
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(routemethod.OnErrorEndPoint))
                            {
                                SendError(routemethod, context);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(routemethod.OnErrorEndPoint))
                        {
                            SendError(routemethod, context);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(routemethod.OnErrorEndPoint))
                    {
                        SendError(routemethod, context);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void SendRetry<TContent, THandler>(RouteMethod<TContent, THandler> routemethod, InboundMessageContext<TContent> context, IRetryPolicy policy)
            where THandler : class
        {
            var options = new Options()
            {
                EndPointName = routemethod.OnRetryEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(policy.NextRetryInterval(context.RetryCount + 1)),
                RetryCount = context.RetryCount + 1,
                Saga = context.Saga
            };

            _bus.Send(context.Content, context.Origin, options);
        }

        private void SendError<TContent, THandler>(RouteMethod<TContent, THandler> routemethod, InboundMessageContext<TContent> context)
            where THandler : class
        {
            var options = new Options()
            {
                EndPointName = routemethod.OnErrorEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                Saga = context.Saga,
            };

            _bus.Send(context.Content, context.Origin, options);


        }
    }
}