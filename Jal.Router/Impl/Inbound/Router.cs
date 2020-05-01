using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Router : IRouter
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly IPipelineBuilder _pipeline;

        private readonly ILogger _logger;

        public Router(IComponentFactoryFacade factory, IPipelineBuilder pipeline, ILogger logger)
        {
            _factory = factory;

            _pipeline = pipeline;

            _logger = logger;
        }
        public async Task Route(MessageContext context)
        {
            var interceptor = _factory.CreateRouterInterceptor();

            interceptor.OnEntry(context);

            var when = true;

            if (context.Route.Condition != null)
            {
                when = context.Route.Condition(context);
            }

            if (when)
            {

                try
                {
                    var chain = _pipeline.ForAsync<MessageContext>();

                    foreach (var type in _factory.Configuration.RouteMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in context.Route.Middlewares)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync(context.Route.Middleware).RunAsync(context).ConfigureAwait(false);

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
