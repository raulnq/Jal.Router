using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
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
                if (metadata.Parameters["endpoint"] is string endpointname && !string.IsNullOrEmpty(endpointname))
                {
                    var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, context.IdentityContext, context.Route, context.Saga, context.Version);

                    var serializer = _factory.CreateMessageSerializer();

                    var content = serializer.Deserialize(context.ContentContext.Data, context.ContentContext.Type);

                    _logger.Log($"Message {context.Id}, forwarding the message to the endpoint {endpointname} by route {context.Name}");

                    await context.Send(content, context.Origin, options);
                }
            }
        }
    }
}