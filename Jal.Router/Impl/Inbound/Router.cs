using System;
using System.Collections.Generic;
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
        public void Route(object message, Route route)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            var interceptor = _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);

            var context = adapter.Read(message, route.ContentType, route.UseClaimCheck);

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
}
