using System;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ForwardRouteEntryMessageHandler : IRouteEntryMessageHandler
    {
        private readonly ILogger _logger;

        public ForwardRouteEntryMessageHandler(ILogger logger)
        {
            _logger = logger;
        }
        public Task Handle(MessageContext context, Handler metadata)
        {
            if (metadata.Parameters.ContainsKey("endpoint") && metadata.Parameters["endpoint"] is string endpointname && !string.IsNullOrEmpty(endpointname) 
                && metadata.Parameters.ContainsKey("type") && metadata.Parameters["type"] is Type type && type!=null)
            {
                var content = context.Deserialize(context.ContentContext.Data, type);

                _logger.Log($"Message {context.Id}, forwarding the message to the endpoint {endpointname} by route {context.Name}");

                return context.Send(content, endpointname);
            }

            return Task.CompletedTask;
        }
    }
}