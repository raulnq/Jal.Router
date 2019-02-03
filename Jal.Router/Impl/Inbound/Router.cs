using System;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class Router : IRouter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IPipelineBuilder _pipeline;

        private readonly ILogger _logger;

        public Router(IComponentFactory factory, IConfiguration configuration, IPipelineBuilder pipeline, ILogger logger)
        {
            _factory = factory;

            _configuration = configuration;

            _pipeline = pipeline;

            _logger = logger;
        }
        public void Route<TMiddleware>(MessageContext context) where TMiddleware : IMiddleware<MessageContext>
        {
            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            interceptor.OnEntry(context);

            var name = context.Saga == null ? context.Route.Name : context.Saga.Name + "/" + context.Route.Name;

            var when = true;

            if (context.Route.When != null)
            {
                when = context.Route.When(context);
            }

            if (when)
            {

                try
                {
                    var chain = _pipeline.For<MessageContext>().Use<MessageExceptionHandler>();

                    foreach (var type in _configuration.InboundMiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    foreach (var type in context.Route.MiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    chain.Use<TMiddleware>().Run(context);

                    interceptor.OnSuccess(context);
                }
                catch (Exception ex)
                {
                    interceptor.OnException(context, ex);

                    throw;
                }
                finally
                {
                    interceptor.OnExit(context);

                    _logger.Log($"Message {context.IdentityContext.Id} handled by route {name}");
                }

            }
            else
            {
                

                _logger.Log($"Message {context.IdentityContext.Id} skipped by route {name}");
            }
        }
    }
}
