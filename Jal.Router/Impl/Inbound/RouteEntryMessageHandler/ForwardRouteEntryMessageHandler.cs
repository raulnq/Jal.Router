using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.RouteEntryMessageHandler
{

    public class ForwardRouteEntryMessageHandler : IRouteEntryMessageHandler
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public ForwardRouteEntryMessageHandler(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task Handle(MessageContext context, Handler metadata)
        {
            if (metadata.Parameters.ContainsKey("endpoint"))
            {
                var endpointname = metadata.Parameters["endpoint"] as string;

                var options = new Options(endpointname, context.CopyHeaders(), context.SagaContext, context.TrackingContext.Tracks, context.IdentityContext, context.Route, context.Version);

                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize(context.ContentContext.Data, context.ContentContext.Type);

                _logger.Log($"Message {context.IdentityContext.Id}, forwarding the message to the endpoint {endpointname} by route {context.GetFullName()}");

                await context.Send(content, context.Origin, options);
            }
        }
    }
}