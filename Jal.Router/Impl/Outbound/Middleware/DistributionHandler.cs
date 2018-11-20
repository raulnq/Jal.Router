using System;
using System.Linq;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{

    public class DistributionHandler : IMiddleware<MessageContext>
    {
        private readonly ILogger _logger;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;
        public DistributionHandler(ILogger logger, IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;

            _configuration = configuration;

            _logger = logger;
        }
 
        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var shuffler = _factory.Create<IChannelShuffler>(_configuration.ChannelShufflerType);

            var channels = shuffler.Shuffle(context.Data.EndPoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            var index = context.Index;

            foreach (var channel in channels)
            {
                context.Data.Channel = channel;

                try
                {
                    count++;

                    next(context);

                    return;
                }
                catch (Exception)
                {
                    if (count < numberofchannels)
                    {
                        context.Index = index;

                        _logger.Log($"Message {context.Data.Identity.Id} failed to distribute ({count}), moving to the next channel");
                    }
                    else
                    {
                        _logger.Log($"Message {context.Data.Identity.Id} failed to distribute ({count}), no more channels");

                        throw;
                    }
                }
            }
        }
    }
}