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
        public object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext)
        {
            var shuffler = _factory.Create<IChannelShuffler>(_configuration.ChannelShufflerType);

            var channels = shuffler.Shuffle(context.EndPoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            var index = middlewarecontext.Index;

            foreach (var channel in channels)
            {
                middlewarecontext.Channel = channel;

                try
                {
                    count++;

                    return next(context, middlewarecontext);
                }
                catch (Exception)
                {
                    if(count < numberofchannels)
                    {
                        middlewarecontext.Index = index;

                        _logger.Log($"Message {context.Identity.Id} failed to distribute ({count}), moving to the next channel");
                    }
                    else
                    {
                        _logger.Log($"Message {context.Identity.Id} failed to distribute ({count}), no more channels");

                        throw;
                    }
                }
            }

            return null;
        }
    }
}