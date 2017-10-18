using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var bodyadapter = _factory.Create<IMessageBodyAdapter>(_configuration.MessageBodyAdapterType);

            var metadataadapter = _factory.Create<IMessageMetadataAdapter>(_configuration.MessageMetadataAdapterType);

            var logger = _factory.Create<IRouterLogger>(_configuration.RouterLoggerType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = metadataadapter.Create(message);

            logger.OnEntry(context);

            interceptor.OnEntry(context);

            try
            {
                var contenttype = typeof(TContent);

                var saga = _provider.Provide(saganame);

                if (saga != null)
                {
                    if (saga.StartingRoute != null && saga.StartingRoute.BodyType == contenttype)
                    {
                        var content = bodyadapter.Read<TContent, TMessage>(message);

                        var contextpluscontent = new IndboundMessageContext<TContent>(context, content);

                        var middlewares = new List<Type>(_configuration.FilterTypes);

                        middlewares.AddRange(saga.StartingRoute.FilterTypes);

                        middlewares.Add(typeof(MessageExceptionHandler));

                        middlewares.Add(typeof(StartingMessageHandler));

                        var parameter = new MiddlewareParameter() { Route = saga.StartingRoute, Saga = saga};

                        var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), contextpluscontent, parameter);

                        pipeline.Excute();

                        logger.OnSuccess(context, content);

                        interceptor.OnSuccess(context, content);
                    }
                    else
                    {
                        var routes = _provider.Provide(saga.NextRoutes.ToArray(), contenttype, routename);

                        if (routes != null && routes.Length > 0)
                        {
                            var content = bodyadapter.Read<TContent, TMessage>(message);

                            foreach (var route in routes)
                            {
                                var contextpluscontent = new IndboundMessageContext<TContent>(context, content);

                                var middlewares = new List<Type>(_configuration.FilterTypes);

                                middlewares.AddRange(route.FilterTypes);

                                middlewares.Add(typeof(MessageExceptionHandler));

                                middlewares.Add(typeof(NextMessageHandler));

                                var parameter = new MiddlewareParameter() { Route = route, Saga = saga};

                                var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), contextpluscontent, parameter);

                                pipeline.Excute();
                            }

                            logger.OnSuccess(context, content);

                            interceptor.OnSuccess(context, content);
                        }
                        else
                        {
                            throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and route name {routename}");
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"No saga to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and saga name {saganame}");
                }

            }
            catch (Exception ex)
            {
                var inner = ex.InnerException ?? ex;

                logger.OnException(context, inner);

                interceptor.OnException(context, inner);

                throw inner;
            }
            finally
            {
                stopwatch.Stop();

                logger.OnExit(context, stopwatch.ElapsedMilliseconds);

                interceptor.OnExit(context);
            }
        }

        public void Route<TContent, TMessage>(TMessage message, string routename = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var bodyadapter = _factory.Create<IMessageBodyAdapter>(_configuration.MessageBodyAdapterType);

            var logger = _factory.Create<IRouterLogger>(_configuration.RouterLoggerType);

            var metadataadapter = _factory.Create<IMessageMetadataAdapter>(_configuration.MessageMetadataAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = metadataadapter.Create(message);

            logger.OnEntry(context);

            interceptor.OnEntry(context);

            try
            {
                var contenttype = typeof(TContent);

                var routes = _provider.Provide(contenttype, routename);

                var content = bodyadapter.Read<TContent, TMessage>(message);

                if (routes != null && routes.Length > 0)
                {
                    foreach (var route in routes)
                    {
                        var contextpluscontent = new IndboundMessageContext<TContent>(context, content);

                        var middlewares = new List<Type>(_configuration.FilterTypes);

                        middlewares.AddRange(route.FilterTypes);

                        middlewares.Add(typeof(MessageExceptionHandler));

                        middlewares.Add(typeof(MessageHandler));

                        var parameter = new MiddlewareParameter() {Route = route};

                        var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), contextpluscontent, parameter);

                        pipeline.Excute();
                    }

                    logger.OnSuccess(context, content);

                    interceptor.OnSuccess(context, content);
                }
                else
                {
                    throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and route name {routename}");
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException ?? ex;

                logger.OnException(context, inner);

                interceptor.OnException(context, inner);

                throw inner;
            }
            finally
            {
                stopwatch.Stop();

                logger.OnExit(context, stopwatch.ElapsedMilliseconds);

                interceptor.OnExit(context);
            }
        }
    }
}
