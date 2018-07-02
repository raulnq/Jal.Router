using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class DistributionHandler : IMiddleware
    {
        private readonly ILogger _logger;
        public DistributionHandler(ILogger logger)
        {
            _logger = logger;
        }
        public void Execute(MessageContext context, Action next, Action current, MiddlewareParameter parameter)
        {
            var channels = context.EndPoint.Channels.Count;

            var count = 0;

            var @action = next;

            foreach (var channel in context.EndPoint.Channels)
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
                    if(count < channels)
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