using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class SagaRouter : ISagaRouter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public SagaRouter(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;

            _configuration = configuration;
        }

        public void Start(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck);

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

                    middlewares.AddRange(saga.StartingRoute.MiddlewareTypes);

                    middlewares.Add(typeof(StartingMessageHandler));

                    var parameter = new MiddlewareParameter() { Route = route, Saga = saga };

                    context.Route = parameter.Route;

                    context.Saga = saga;

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), context, parameter);

                    pipeline.Execute();

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

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck);

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

                    middlewares.Add(typeof(NextMessageHandler));

                    var parameter = new MiddlewareParameter() { Route = route, Saga = saga };

                    context.Route = route;

                    context.Saga = saga;

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), context, parameter);

                    pipeline.Execute();

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

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck);

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

                    middlewares.AddRange(saga.StartingRoute.MiddlewareTypes);

                    middlewares.Add(typeof(EndingMessageHandler));

                    var parameter = new MiddlewareParameter() { Route = route, Saga = saga };

                    context.Route = parameter.Route;

                    context.Saga = saga;

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), context, parameter);

                    pipeline.Execute();

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