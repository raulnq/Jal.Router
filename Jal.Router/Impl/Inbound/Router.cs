using System;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{

    public class Router : IRouter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Router(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;

            _configuration = configuration;
        }

        public void RouteToStartingSaga(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType);

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

        public void RouteToContinueSaga(object message, Saga saga, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType);

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

        public void Route(object message, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType);

            interceptor.OnEntry(context);

            try
            {
                var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                middlewares.AddRange(route.MiddlewareTypes);

                middlewares.Add(typeof(MessageHandler));

                var parameter = new MiddlewareParameter() { Route = route };

                context.Route = route;

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
