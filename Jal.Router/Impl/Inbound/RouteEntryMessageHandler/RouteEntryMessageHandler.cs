using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.RouteEntryMessageHandler
{
    public class RouteEntryMessageHandler : IRouteEntryMessageHandler
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public RouteEntryMessageHandler(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task Handle(MessageContext context, Handler metadata)
        {
            if (metadata.Parameters.ContainsKey("init"))
            {
                var fallback = metadata.Parameters["init"] as Func<MessageContext, Handler, Task>;

                _logger.Log($"Message {context.IdentityContext.Id}, initiator executed by route {context.GetFullName()}");

                await fallback(context, metadata);
            }
        }
    }
}