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
            return default(MessageContext);
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route)
        {
            return Task.FromResult(default(MessageContext));
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint)
        {
            return Task.FromResult(default(MessageContext));
        }
    }
}