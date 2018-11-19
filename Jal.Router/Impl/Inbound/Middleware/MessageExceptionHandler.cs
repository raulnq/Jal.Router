using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class MessageExceptionHandler : IMiddleware<MessageContext>
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

        private void SendRetry(Route route, MessageContext context, IRetryPolicy policy)
        {
            var options = new Options()
            {
                EndPointName = route.OnRetryEndPoint,
                Headers = context.Headers,
                Identity = context.Identity,
                Version = context.Version,
                ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(policy.NextRetryInterval(context.RetryCount + 1)),
                RetryCount = context.RetryCount + 1,
                SagaContext = context.SagaContext,
            };

            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var content = adapter.Deserialize(context.Content, context.ContentType);

            _bus.Send(content, context.Origin, options);
        }

        private void SendError(Route route, MessageContext context, Exception ex)
        {
            var options = new Options()
            {
                EndPointName = route.OnErrorEndPoint,
                Headers = context.Headers,
                Identity = context.Identity,
                Version = context.Version,
                SagaContext = context.SagaContext,
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

            var content = adapter.Deserialize(context.Content, context.ContentType);

            _bus.Send(content, context.Origin, options);


        }

        private IRetryPolicy GetRetryPolicy(Route route)
        {
            var extractor = _factory.Create<IValueFinder>(route.RetryExtractorType);

            return route.RetryPolicyExtractor(extractor);
        }
        private bool HasRetry(Route route)
        {
            return route.RetryExceptionType != null && route.RetryPolicyExtractor != null;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            IRetryPolicy policy = null;

            var messagecontext = context.Data;

            try
            {
                if (HasRetry(messagecontext.Route))
                {
                    policy = GetRetryPolicy(messagecontext.Route);

                    if (policy != null)
                    {
                        var interval = policy.NextRetryInterval(messagecontext.RetryCount + 1);

                        messagecontext.LastRetry = !policy.CanRetry(messagecontext.RetryCount + 1, interval);
                    }
                    else
                    {
                        throw new ApplicationException("The retry policy cannot be null");
                    }
                }

                next(context);
            }
            catch (Exception ex)
            {
                if (policy != null)
                {
                    if (messagecontext.Route.RetryExceptionType == ex.GetType() || (ex.InnerException != null && messagecontext.Route.RetryExceptionType == ex.InnerException.GetType()))
                    {
                        if (!messagecontext.LastRetry)
                        {
                            if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnRetryEndPoint))
                            {
                                SendRetry(messagecontext.Route, messagecontext, policy);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                                {
                                    SendError(messagecontext.Route, messagecontext, ex);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                            {
                                SendError(messagecontext.Route, messagecontext, ex);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                        {
                            SendError(messagecontext.Route, messagecontext, ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                    {
                        SendError(messagecontext.Route, messagecontext, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}