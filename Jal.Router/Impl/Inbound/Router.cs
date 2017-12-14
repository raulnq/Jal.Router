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
        //private readonly IRouteProvider _provider;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Router(IComponentFactory factory, IConfiguration configuration)
        {
            //_provider = provider;

            _factory = factory;

            _configuration = configuration;
        }

        //public void RouteToSaga<TContent, TMessage>(TMessage message, string saganame)
        //{
        //    var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

        //    var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

        //    var context = adapter.Read<TContent, TMessage>(message);

        //    interceptor.OnEntry(context);

        //    try
        //    {
        //        var saga = _provider.Provide(saganame);

        //        if (saga != null)
        //        {
        //            if (saga.StartingRoute != null && saga.StartingRoute.ContentType == context.ContentType)
        //            {
        //                var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

        //                middlewares.AddRange(_configuration.InboundMiddlewareTypes);

        //                middlewares.AddRange(saga.StartingRoute.MiddlewareTypes);

        //                middlewares.Add(typeof(StartingMessageHandler));

        //                var parameter = new MiddlewareParameter() { Route = saga.StartingRoute, Saga = saga};

        //                context.Route = parameter.Route;

        //                context.Saga = saga;

        //                var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

        //                pipeline.Execute();

        //                interceptor.OnSuccess(context);
        //            }
        //            else
        //            {
        //                var routes = _provider.Provide(saga.NextRoutes.ToArray(), context.ContentType);

        //                if (routes != null && routes.Length > 0)
        //                {
        //                    foreach (var route in routes)
        //                    {
        //                        var middlewares = new List<Type> {typeof (MessageExceptionHandler)};

        //                        middlewares.AddRange(_configuration.InboundMiddlewareTypes);

        //                        middlewares.AddRange(route.MiddlewareTypes);

        //                        middlewares.Add(typeof(NextMessageHandler));

        //                        var parameter = new MiddlewareParameter() { Route = route, Saga = saga};

        //                        context.Route = route;

        //                        context.Saga = saga;

        //                        var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

        //                        pipeline.Execute();
        //                    }

        //                    interceptor.OnSuccess(context);
        //                }
        //                else
        //                {
        //                    throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName}");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new ApplicationException($"No saga to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName}  and saga name {saganame}");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);

        //        interceptor.OnException(context, ex);

        //        throw;
        //    }
        //    finally
        //    {
        //        interceptor.OnExit(context);
        //    }
        //}

        public void Route<TContent, TMessage>(TMessage message, Saga saga, Route route, bool startingroute)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read<TContent, TMessage>(message);

            interceptor.OnEntry(context);

            try
            {
                if (startingroute)
                {
                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(saga.StartingRoute.MiddlewareTypes);

                    middlewares.Add(typeof(StartingMessageHandler));

                    var parameter = new MiddlewareParameter() { Route = route, Saga = saga };

                    context.Route = parameter.Route;

                    context.Saga = saga;

                    var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

                    pipeline.Execute();

                    interceptor.OnSuccess(context);
                }
                else
                {

                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(route.MiddlewareTypes);

                    middlewares.Add(typeof(NextMessageHandler));

                    var parameter = new MiddlewareParameter() { Route = route, Saga = saga };

                    context.Route = route;

                    context.Saga = saga;

                    var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

                    pipeline.Execute();

                    interceptor.OnSuccess(context);
                }

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

        public void Route<TContent, TMessage>(TMessage message, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read<TContent, TMessage>(message);

            interceptor.OnEntry(context);

            try
            {
                var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                middlewares.AddRange(route.MiddlewareTypes);

                middlewares.Add(typeof(MessageHandler));

                var parameter = new MiddlewareParameter() { Route = route };

                context.Route = route;

                var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

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
    
        //public void Route<TContent, TMessage>(TMessage message)
        //{
        //    var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

        //    var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

        //    var context = adapter.Read<TContent, TMessage>(message);

        //    interceptor.OnEntry(context);

        //    try
        //    {
        //        var routes = _provider.Provide(context.ContentType);

        //        if (routes != null && routes.Length > 0)
        //        {
        //            foreach (var route in routes)
        //            {
        //                var middlewares = new List<Type> {typeof (MessageExceptionHandler)};

        //                middlewares.AddRange(_configuration.InboundMiddlewareTypes);

        //                middlewares.AddRange(route.MiddlewareTypes);

        //                middlewares.Add(typeof(MessageHandler));

        //                var parameter = new MiddlewareParameter() {Route = route};

        //                context.Route = route;

        //                var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), context, parameter);

        //                pipeline.Execute();
        //            }

        //            interceptor.OnSuccess(context);
        //        }
        //        else
        //        {
        //            throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {context.ContentType.FullName}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);

        //        interceptor.OnException(context, ex);

        //        throw;
        //    }
        //    finally
        //    {
        //        interceptor.OnExit(context);
        //    }
        //}
    }
}
