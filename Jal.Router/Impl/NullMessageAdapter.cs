using System.Threading.Tasks;
using Jal.Router.Interface.Inbound;
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
            return new MessageContext(new EndPoint(string.Empty), new Options());
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route, Channel channel, Saga saga = null)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options()));
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options()));
        }
    }
}