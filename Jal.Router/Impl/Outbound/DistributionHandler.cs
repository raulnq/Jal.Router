using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class DistributionHandler : IMiddleware
    {
        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var channels = context.EndPoint.Channels.Count;

            var count = 0;

            foreach (var channel in context.EndPoint.Channels)
            {
                parameter.Channel = channel;

                try
                {
                    count++;
                    next();
                    break;
                }
                catch (Exception ex)
                {
                    if(count < channels)
                    {
                        Console.WriteLine($"Message {context.Id} failed to distribute ({count}), moving to the next channel {ex}");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}