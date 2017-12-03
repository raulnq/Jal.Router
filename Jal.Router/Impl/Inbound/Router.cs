using System;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;
namespace Jal.Router.Impl.Inbound
{
    public class Router : IRouter
    {
        private readonly IRouteProvider _provider;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Router(IRouteProvider provider, IComponentFactory factory, IConfiguration configuration)
        {
            _provider = provider;

            _factory = factory;

            _configuration = configuration;
        }

        public void RouteToSaga<TContent, TMessage>(TMessage message, string saganame, string routename = "")
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read<TContent, TMessage>(message);

            interceptor.OnEntry(context);

            try
            {
                var saga = _provider.Provide(saganame);

                if (saga != null)
                {
                    if (saga.StartingRoute != null && saga.StartingRoute.BodyType == context.ContentType)
                    {
                        var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                        middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                        middlewares.AddRange(saga.StartingRoute.MiddlewareTypes);

                        middlewares.Add(typeof(StartingMessageHandler));

                        var parameter = new MiddlewareParameter() { Route = saga.StartingRoute, Saga = saga};

                        context.Route = parameter.Route;

                        context.Saga = saga;

                        var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

                        pipeline.Execute();

                        interceptor.OnSuccess(context);
                    }
                    else
                    {
                        var routes = _provider.Provide(saga.NextRoutes.ToArray(), context.ContentType, routename);

                        if (routes != null && routes.Length > 0)
                        {
                            foreach (var route in routes)
                            {
                                var middlewares = new List<Type> {typeof (MessageExceptionHandler)};

                                middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                                middlewares.AddRange(route.MiddlewareTypes);

                                middlewares.Add(typeof(NextMessageHandler));

                                var parameter = new MiddlewareParameter() { Route = route, Saga = saga};

                                context.Route = route;

                                context.Saga = saga;

                                var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

                                pipeline.Execute();
                            }

                            interceptor.OnSuccess(context);
                        }
                        else
                        {
                            throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName} and route name {routename}");
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"No saga to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName}  and saga name {saganame}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                interceptor.OnException(context, ex);

                throw;
            }
            finally
            {
                interceptor.OnExit(context);
            }
        }

        public void Route<TContent, TMessage>(TMessage message, string routename = "")
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read<TContent, TMessage>(message);

            interceptor.OnEntry(context);

            try
            {
                var routes = _provider.Provide(context.ContentType, routename);

                if (routes != null && routes.Length > 0)
                {
                    foreach (var route in routes)
                    {
                        var middlewares = new List<Type> {typeof (MessageExceptionHandler)};

                        middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                        middlewares.AddRange(route.MiddlewareTypes);

                        middlewares.Add(typeof(MessageHandler));

                        var parameter = new MiddlewareParameter() {Route = route};

                        context.Route = route;

                        var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

                        pipeline.Execute();
                    }

                    interceptor.OnSuccess(context);
                }
                else
                {
                    throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName} and route name {routename}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

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
