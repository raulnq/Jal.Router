using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class BusMiddleware : IAsyncMiddleware<MessageContext>
    {
        private readonly ILogger _logger;

        private readonly IComponentFactoryFacade _factory;

        public BusMiddleware(ILogger logger, IComponentFactoryFacade factory)
        {
            _factory = factory;

            _logger = logger;
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            foreach (var item in messagecontext.EndPoint.EntryHandlers)
            {
                var handler = _factory.CreateBusEntryMessageHandler(item.Type);

                await handler.Handle(messagecontext, item).ConfigureAwait(false);
            }

            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var errormetadata = messagecontext.EndPoint.ErrorHandlers.Where(x => x.ExceptionTypes.Count == 0 || x.ExceptionTypes.Contains(ex.GetType()));

                var handled = false;

                foreach (var item in errormetadata)
                {
                    var handler = _factory.CreateBusErrorMessageHandler(item.Type);

                    handled = await handler.Handle(messagecontext, ex, item).ConfigureAwait(false);

                    if (handled)
                    {
                        break;
                    }
                }

                if (!handled)
                {
                    _logger.Log($"Message {messagecontext.Id} with an exception not distribute by endpoint {messagecontext.Name}");

                    throw;
                }
            }
            finally
            {
                foreach (var item in messagecontext.EndPoint.ExitHandlers)
                {
                    var handler = _factory.CreateBusExitMessageHandler(item.Type);

                    await handler.Handle(messagecontext, item).ConfigureAwait(false);
                }
            }
            
        }
    }
}