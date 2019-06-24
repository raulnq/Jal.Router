using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{

    public class BusMessageHandler : IMiddlewareAsync<MessageContext>
    {
        private readonly ILogger _logger;

        private readonly IComponentFactoryGateway _factory;

        public BusMessageHandler(ILogger logger, IComponentFactoryGateway factory)
        {
            _factory = factory;

            _logger = logger;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var shuffler = _factory.CreateChannelShuffler();

            var channels = shuffler.Shuffle(context.Data.EndPoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            var index = context.Index;

            foreach (var channel in channels)
            {
                context.Data.UpdateFromEndpoint(channel);

                try
                {
                    count++;

                    await next(context);

                    return;
                }
                catch (Exception)
                {
                    if (count < numberofchannels)
                    {
                        context.Index = index;

                        _logger.Log($"Message {context.Data.IdentityContext.Id} failed to distribute ({count}), moving to the next channel");
                    }
                    else
                    {
                        _logger.Log($"Message {context.Data.IdentityContext.Id} failed to distribute ({count}), no more channels");

                        throw;
                    }
                }
            }
        }
    }
}