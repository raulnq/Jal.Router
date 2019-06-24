using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{

    public class RouteMiddleware : IMiddlewareAsync<MessageContext>
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public RouteMiddleware(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            foreach (var item in messagecontext.Route.EntryHandlers)
            {
                var handler = _factory.CreateRouteEntryMessageHandler(item.Type);

                await handler.Handle(messagecontext, item).ConfigureAwait(false);
            }

            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var errormetadata = messagecontext.Route.ErrorHandlers.Where(x=> x.ExceptionTypes.Count == 0 || x.ExceptionTypes.Contains(ex.GetType()));

                var handled = false;

                foreach (var item in errormetadata)
                {
                    var handler = _factory.CreateRouteErrorMessageHandler(item.Type);

                    handled = await handler.OnException(messagecontext, ex, item).ConfigureAwait(false);

                    if (handled)
                    {
                        break;
                    }
                }

                if(!handled)
                {
                    _logger.Log($"Message {messagecontext.IdentityContext.Id} with an exception not handled by route {messagecontext.GetFullName()}");

                    throw;
                }
            }
            finally
            {
                foreach (var item in messagecontext.Route.ExitHandlers)
                {
                    var handler = _factory.CreateRouteExitMessageHandler(item.Type);

                    await handler.Handle(messagecontext, item).ConfigureAwait(false);
                }
            }
        }
    }
}