using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using IMiddleware = Jal.Router.Interface.Inbound.IMiddleware;

namespace Jal.Router.Impl.Inbound
{
    public class MessageExceptionHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IBus _bus;

        public MessageExceptionHandler(IComponentFactory factory, IBus bus)
        {
            _factory = factory;
            _bus = bus;
        }

        public void Execute<TContent>(MessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            IRetryPolicy policy = null;

            try
            {
                if (HasRetry(parameter.Route))
                {
                    policy = GetRetryPolicy(parameter.Route);

                    if (policy != null)
                    {
                        var interval = policy.NextRetryInterval(context.RetryCount + 1);

                        context.LastRetry = !policy.CanRetry(context.RetryCount + 1, interval);
                    }
                    else
                    {
                        Console.WriteLine("The retry policy cannot be null");
                        throw new ApplicationException("The retry policy cannot be null");
                    }
                }

                next();
            }
            catch (Exception ex)
            {
                if (policy != null)
                {
                    if (parameter.Route.RetryExceptionType == ex.GetType())
                    {
                        if (!context.LastRetry)
                        {
                            if (!string.IsNullOrWhiteSpace(parameter.Route.OnRetryEndPoint))
                            {
                                SendRetry(parameter.Route, context, policy);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(parameter.Route.OnErrorEndPoint))
                                {
                                    SendError(parameter.Route, context, ex);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(parameter.Route.OnErrorEndPoint))
                            {
                                SendError(parameter.Route, context, ex);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(parameter.Route.OnErrorEndPoint))
                        {
                            SendError(parameter.Route, context, ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(parameter.Route.OnErrorEndPoint))
                    {
                        SendError(parameter.Route, context, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void SendRetry<TContent>(Route route, MessageContext<TContent> context, IRetryPolicy policy)
        {
            var options = new Options()
            {
                EndPointName = route.OnRetryEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(policy.NextRetryInterval(context.RetryCount + 1)),
                RetryCount = context.RetryCount + 1,
                SagaInfo = context.SagaInfo
            };

            _bus.Send(context.Content, context.Origin, options);
        }

        private void SendError<TContent>(Route route, MessageContext<TContent> context, Exception ex)
        {
            var options = new Options()
            {
                EndPointName = route.OnErrorEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                SagaInfo = context.SagaInfo,
            };
            if (ex != null)
            {
                options.Headers.Add("exceptionmessage", ex.Message);
                options.Headers.Add("exceptionstacktrace", ex.StackTrace);
                if (ex.InnerException!=null)
                {
                    options.Headers.Add("innerexceptionmessage", ex.InnerException.Message);
                    options.Headers.Add("innerexceptionstacktrace", ex.InnerException.StackTrace);
                }
            }
                
            _bus.Send(context.Content, context.Origin, options);


        }

        private IRetryPolicy GetRetryPolicy(Route route)
        {
            var extractor = _factory.Create<IValueSettingFinder>(route.RetryExtractorType);

            return route.RetryPolicyExtractor(extractor);
        }
        private bool HasRetry(Route route)
        {
            return route.RetryExceptionType != null && route.RetryPolicyExtractor != null;
        }
    }
}