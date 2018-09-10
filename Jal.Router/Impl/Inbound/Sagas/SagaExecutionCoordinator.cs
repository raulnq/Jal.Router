using System;
using System.Collections.Generic;
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

        private readonly IPipeline _pipeline;

        public SagaExecutionCoordinator(IComponentFactory factory, IConfiguration configuration, IPipeline pipeline)
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
                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(saga.FirstRoute.MiddlewareTypes);

                    middlewares.Add(typeof(FirstMessageHandler));

                    context.Route = route;

                    context.Saga = saga;

                    _pipeline.Execute(middlewares.ToArray(), context);

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

                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(route.MiddlewareTypes);

                    middlewares.Add(typeof(MiddleMessageHandler));

                    context.Route = route;

                    context.Saga = saga;

                    _pipeline.Execute(middlewares.ToArray(), context);

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
                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(saga.FirstRoute.MiddlewareTypes);

                    middlewares.Add(typeof(LastMessageHandler));

                    context.Route = route;

                    context.Saga = saga;

                    _pipeline.Execute(middlewares.ToArray(), context);

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