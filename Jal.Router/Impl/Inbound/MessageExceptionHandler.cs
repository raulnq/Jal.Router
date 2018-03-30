using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
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

        private readonly IConfiguration _configuration;

        public MessageExceptionHandler(IComponentFactory factory, IBus bus, IConfiguration configuration)
        {
            _factory = factory;
            _bus = bus;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
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

        private void SendRetry(Route route, MessageContext context, IRetryPolicy policy)
        {
            var options = new Options()
            {
                EndPointName = route.OnRetryEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(policy.NextRetryInterval(context.RetryCount + 1)),
                RetryCount = context.RetryCount + 1,
                SagaContext = context.SagaContext,
                ReplyToRequestId = context.ReplyToRequestId,
                RequestId = context.RequestId,
            };

            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var content = adapter.Deserialize(context.ContentAsString, context.ContentType);

            _bus.Send(content, context.Origin, options);
        }

        private void SendError(Route route, MessageContext context, Exception ex)
        {
            var options = new Options()
            {
                EndPointName = route.OnErrorEndPoint,
                Headers = context.Headers,
                Id = context.Id,
                Version = context.Version,
                SagaContext = context.SagaContext,
                ReplyToRequestId = context.ReplyToRequestId,
                RequestId = context.RequestId,
            };
            if (ex != null)
            {
                if (options.Headers.ContainsKey("exceptionmessage"))
                {
                    options.Headers["exceptionmessage"] = ex.Message;
                }
                else
                {
                    options.Headers.Add("exceptionmessage", ex.Message);
                }
                if (options.Headers.ContainsKey("exceptionstacktrace"))
                {
                    options.Headers["exceptionstacktrace"] = ex.StackTrace;
                }
                else
                {
                    options.Headers.Add("exceptionstacktrace", ex.StackTrace);
                }
                if (ex.InnerException!=null)
                {
                    if (options.Headers.ContainsKey("innerexceptionmessage"))
                    {
                        options.Headers["innerexceptionmessage"] = ex.InnerException.Message;
                    }
                    else
                    {
                        options.Headers.Add("innerexceptionmessage", ex.InnerException.Message);
                    }
                    if (options.Headers.ContainsKey("innerexceptionstacktrace"))
                    {
                        options.Headers["innerexceptionstacktrace"] = ex.InnerException.StackTrace;
                    }
                    else
                    {
                        options.Headers.Add("innerexceptionstacktrace", ex.InnerException.StackTrace);
                    }
                }
            }

            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var content = adapter.Deserialize(context.ContentAsString, context.ContentType);

            _bus.Send(content, context.Origin, options);


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