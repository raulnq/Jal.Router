using System;
using System.Linq;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class MessageExceptionHandler : IMiddleware<MessageContext>
    {
        private readonly IComponentFactory _factory;

        private readonly IBus _bus;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public MessageExceptionHandler(IComponentFactory factory, IBus bus, IConfiguration configuration, ILogger logger)
        {
            _factory = factory;
            _bus = bus;
            _configuration = configuration;
            _logger = logger;
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
            var finder = _factory.Create<IValueFinder>(route.RetryValueFinderType);

            return route.RetryPolicyProvider(finder);
        }

        private bool CanHandle(Route route, Exception ex)
        {
            var retry = route.RetryExceptionTypes.FirstOrDefault(x=>x == ex.GetType());

            if(retry!=null)
            {
                return true;
            }

            if(ex.InnerException != null)
            {
                return route.RetryExceptionTypes.FirstOrDefault(x => x == ex.InnerException.GetType())!=null;
            }

            return false;
        }

        private bool HasRetry(Route route)
        {
            return route.RetryExceptionTypes.Count>0 && route.RetryPolicyProvider != null;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            IRetryPolicy policy = null;

            var messagecontext = context.Data;

            var name = context.Data.Saga == null ? context.Data.Route.Name : context.Data.Saga.Name + "/" + context.Data.Route.Name;

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
                    if (CanHandle(messagecontext.Route, ex))
                    {
                        if (!messagecontext.LastRetry)
                        {
                            if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnRetryEndPoint))
                            {
                                _logger.Log($"Message {context.Data.Identity.Id} sending the message to the retry endpoint {messagecontext.Route.OnRetryEndPoint} retry count {messagecontext.RetryCount +1} by route {name}");

                                SendRetry(messagecontext.Route, messagecontext, policy);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                                {
                                    _logger.Log($"Message {context.Data.Identity.Id} policy without retry endpoint, sending the message to the error endpoint {messagecontext.Route.OnErrorEndPoint} by route {name}");

                                    SendError(messagecontext.Route, messagecontext, ex);
                                }
                                else
                                {
                                    _logger.Log($"Message {context.Data.Identity.Id} policy without retry endpoint by route {name}");

                                    throw;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                            {
                                _logger.Log($"Message {context.Data.Identity.Id} no more retries for the policy, sending the message to the error endpoint {messagecontext.Route.OnErrorEndPoint} by route {name}");

                                SendError(messagecontext.Route, messagecontext, ex);
                            }
                            else
                            {
                                _logger.Log($"Message {context.Data.Identity.Id} no more retries for the policy by route {name}");

                                throw;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                        {
                            _logger.Log($"Message {context.Data.Identity.Id} with an exeception not handled by the retry policy, sending the message to the error endpoint {messagecontext.Route.OnErrorEndPoint} by route {name}");

                            SendError(messagecontext.Route, messagecontext, ex);
                        }
                        else
                        {
                            _logger.Log($"Message {context.Data.Identity.Id} with an exeception not handled by the retry policy by route {name}");

                            throw;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(messagecontext.Route.OnErrorEndPoint))
                    {
                        _logger.Log($"Message {context.Data.Identity.Id} without retry policy, sending the message to the error endpoint {messagecontext.Route.OnErrorEndPoint} by route {name}");

                        SendError(messagecontext.Route, messagecontext, ex);
                    }
                    else
                    {
                        _logger.Log($"Message {context.Data.Identity.Id} without retry policy by route {name}");

                        throw;
                    }
                }
            }
        }
    }
}