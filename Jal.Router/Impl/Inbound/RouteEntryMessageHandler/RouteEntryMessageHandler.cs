using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RouteEntryMessageHandler : IRouteEntryMessageHandler
    {
        private readonly ILogger _logger;

        public RouteEntryMessageHandler(ILogger logger)
        {
            _logger = logger;
        }
        public async Task Handle(MessageContext context, Handler metadata)
        {
            if (metadata.Parameters.ContainsKey("init") && metadata.Parameters["init"] is Func<MessageContext, Handler, Task> fallback && fallback!=null)
            {
                _logger.Log($"Message {context.Id}, initiator executed by route {context.Name}");

                await fallback(context, metadata);
            }
        }
    }
}