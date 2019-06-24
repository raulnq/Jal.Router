using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class NullMessageAdapter : IMessageAdapter
    {
        public Task<object> WriteMetadataAndContent(MessageContext context, EndPoint endpoint)
        {
            return Task.FromResult(default(object));
        }

        public MessageContext ReadMetadata(object message)
        {
            return new MessageContext(new EndPoint(string.Empty), new Options(string.Empty, new System.Collections.Generic.Dictionary<string, string>(), new SagaContext(), new System.Collections.Generic.List<Track>(), new IdentityContext(string.Empty), null, "1"), DateTime.UtcNow, new Origin());
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route, Channel channel, Saga saga = null)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options(string.Empty, new System.Collections.Generic.Dictionary<string, string>(), new SagaContext(), new System.Collections.Generic.List<Track>(), new IdentityContext(string.Empty), null, "1"), DateTime.UtcNow, new Origin()));
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options(endpoint.Name, new System.Collections.Generic.Dictionary<string, string>(), new SagaContext(), new System.Collections.Generic.List<Track>(), new IdentityContext(string.Empty), null, "1"), DateTime.UtcNow, new Origin()));
        }
    }
}