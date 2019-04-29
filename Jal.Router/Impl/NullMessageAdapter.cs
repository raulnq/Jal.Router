using System;
using System.Threading.Tasks;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{

    public class NullMessageAdapter : IMessageAdapter
    {
        public object WriteMetadataAndContent(MessageContext context, bool useclaimcheck)
        {
            return null;
        }

        public object Deserialize(string content, Type type)
        {
            return null;
        }

        public TContent Deserialize<TContent>(string content)
        {
            return default(TContent);
        }

        public string Serialize(object content)
        {
            return string.Empty;
        }

        public MessageContext ReadMetadata(object message)
        {
            return new MessageContext(new EndPoint(string.Empty), new Options());
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options()));
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint)
        {
            return Task.FromResult(new MessageContext(new EndPoint(string.Empty), new Options()));
        }
    }
}