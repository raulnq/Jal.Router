using System;
using Jal.ChainOfResponsability.Fluent.Interfaces;
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

        public Router(IComponentFactory factory, IConfiguration configuration, IPipelineBuilder pipeline)
        {
            _factory = factory;

            _configuration = configuration;

            _pipeline = pipeline;
        }
        public void Route(object message, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

            interceptor.OnEntry(context);

            var when = true;

            if (route.When != null)
            {
                when = route.When(context);
            }

            if (when)
            {

                try
                {
                    context.Route = route;

                    var chain = _pipeline.For<MessageContext>().Use<MessageExceptionHandler>();

                    foreach (var type in _configuration.InboundMiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    foreach (var type in route.MiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    chain.Use<MessageHandler>().Run(context);

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
                }

            }
        }
    }
}
