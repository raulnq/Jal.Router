using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{

    public class DistributionHandler : IMiddleware
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
        public void Execute(MessageContext context, Action next, Action current, MiddlewareParameter parameter)
        {
            var shuffler = _factory.Create<IChannelShuffler>(_configuration.ChannelShufflerType);

            var channels = shuffler.Shuffle(context.EndPoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            var @action = next;

            foreach (var channel in channels)
            {
                parameter.Channel = channel;

                try
                {
                    count++;
                    @action();
                    break;
                }
                catch (Exception)
                {
                    if(count < numberofchannels)
                    {
                        @action = current;
                        _logger.Log($"Message {context.Id} failed to distribute ({count}), moving to the next channel");
                    }
                    else
                    {
                        _logger.Log($"Message {context.Id} failed to distribute ({count}), no more channels");
                        throw;
                    }
                }
            }
        }
    }
}