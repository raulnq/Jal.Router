using System;
using System.Collections.Generic;
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

        private readonly IPipeline _pipeline;

        public Router(IComponentFactory factory, IConfiguration configuration, IPipeline pipeline)
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
                    var middlewares = new List<Type> { typeof(MessageExceptionHandler) };

                    middlewares.AddRange(_configuration.InboundMiddlewareTypes);

                    middlewares.AddRange(route.MiddlewareTypes);

                    middlewares.Add(typeof(MessageHandler));

                    context.Route = route;

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
