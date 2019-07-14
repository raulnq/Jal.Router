using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class Router : IRouter
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly IPipelineBuilder _pipeline;

        private readonly ILogger _logger;

        public Router(IComponentFactoryGateway factory, IPipelineBuilder pipeline, ILogger logger)
        {
            _factory = factory;

            _pipeline = pipeline;

            _logger = logger;
        }
        public async Task Route<TMiddleware>(MessageContext context) where TMiddleware : IMiddlewareAsync<MessageContext>
        {
            var interceptor = _factory.CreateRouterInterceptor();

            interceptor.OnEntry(context);

            var when = true;

            if (context.Route.When != null)
            {
                when = context.Route.When(context);
            }

            if (when)
            {

                try
                {
                    var chain = _pipeline.ForAsync<MessageContext>().UseAsync<RouteMiddleware>();

                    foreach (var type in _factory.Configuration.InboundMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in context.Route.MiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync<TMiddleware>().RunAsync(context).ConfigureAwait(false);

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

                    _logger.Log($"Message {context.Id} handled by route {context.Name}");
                }

            }
            else
            {
                _logger.Log($"Message {context.Id} skipped by route {context.Name}");
            }
        }
    }
}
