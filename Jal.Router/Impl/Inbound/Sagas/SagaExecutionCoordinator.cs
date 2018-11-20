using System;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class SagaExecutionCoordinator : ISagaExecutionCoordinator
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IPipelineBuilder _pipeline;

        public SagaExecutionCoordinator(IComponentFactory factory, IConfiguration configuration, IPipelineBuilder pipeline)
        {
            _factory = factory;

            _configuration = configuration;

            _pipeline = pipeline;
        }

        public void Start(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

            var when = true;

            if (route.When != null)
            {
                when = route.When(context);
            }

            if (when)
            {

                interceptor.OnEntry(context);

                try
                {
                    context.Route = route;

                    context.Saga = saga;

                    var chain = _pipeline.For<MessageContext>()
                        .Use<MessageExceptionHandler>();

                    foreach (var type in _configuration.InboundMiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    foreach (var type in route.MiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    chain.Use<FirstMessageHandler>().Run(context);

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

        public void Continue(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

            var when = true;

            if (route.When != null)
            {
                when = route.When(context);
            }

            if (when)
            {
                interceptor.OnEntry(context);

                try
                {
                    context.Route = route;

                    context.Saga = saga;

                    var chain = _pipeline.For<MessageContext>()
                        .Use<MessageExceptionHandler>();

                    foreach (var type in _configuration.InboundMiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    foreach (var type in route.MiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    chain.Use<MiddleMessageHandler>().Run(context);

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

        public void End(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

            var when = true;

            if (route.When != null)
            {
                when = route.When(context);
            }

            if (when)
            {

                interceptor.OnEntry(context);

                try
                {
                    context.Route = route;

                    context.Saga = saga;

                    var chain = _pipeline.For<MessageContext>()
                        .Use<MessageExceptionHandler>();

                    foreach (var type in _configuration.InboundMiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    foreach (var type in route.MiddlewareTypes)
                    {
                        chain.Use(type);
                    }

                    chain.Use<LastMessageHandler>().Run(context);

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