using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RouterMiddleware : IAsyncMiddleware<MessageContext>
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public RouterMiddleware(IComponentFactoryFacade factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
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
                var errorhandlers = messagecontext.Route.ErrorHandlers.Where(x=> x.ExceptionTypes.Count == 0 || x.ExceptionTypes.Contains(ex.GetType()));

                var handled = false;

                foreach (var errorhandler in errorhandlers)
                {
                    var handler = _factory.CreateRouteErrorMessageHandler(errorhandler.Type);

                    handled = await handler.Handle(messagecontext, ex, errorhandler).ConfigureAwait(false);

                    if (handled)
                    {
                        break;
                    }
                }

                if(!handled)
                {
                    _logger.Log($"Message {messagecontext.Id} with an exception not handled by route {messagecontext.Name}");

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