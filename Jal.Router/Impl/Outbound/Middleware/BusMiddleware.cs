using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{

    public class BusMiddleware : IMiddlewareAsync<MessageContext>
    {
        private readonly ILogger _logger;

        private readonly IComponentFactoryGateway _factory;

        public BusMiddleware(ILogger logger, IComponentFactoryGateway factory)
        {
            _factory = factory;

            _logger = logger;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var shuffler = _factory.CreateChannelShuffler();

            var channels = shuffler.Shuffle(context.Data.EndPoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            var index = context.Index;

            foreach (var item in messagecontext.EndPoint.EntryHandlers)
            {
                var handler = _factory.CreateBusEntryMessageHandler(item.Type);

                await handler.Handle(messagecontext, item).ConfigureAwait(false);
            }
            try
            {
                foreach (var channel in channels)
                {
                    context.Data.UpdateChannel(channel);

                    try
                    {
                        count++;

                        await next(context);

                        return;
                    }
                    catch (Exception ex)
                    {
                        if (count < numberofchannels)
                        {
                            context.Index = index;

                            _logger.Log($"Message {context.Data.Id} failed to distribute ({count}), moving to the next channel");
                        }
                        else
                        {
                            var errormetadata = messagecontext.EndPoint.ErrorHandlers.Where(x => x.ExceptionTypes.Count == 0 || x.ExceptionTypes.Contains(ex.GetType()));

                            var handled = false;

                            foreach (var item in errormetadata)
                            {
                                var handler = _factory.CreateBusErrorMessageHandler(item.Type);

                                handled = await handler.OnException(messagecontext, ex, item).ConfigureAwait(false);

                                if (handled)
                                {
                                    break;
                                }
                            }

                            if (!handled)
                            {
                                _logger.Log($"Message {context.Data.Id} failed to distribute ({count}), no more channels");

                                throw;
                            }
                        }
                    }
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