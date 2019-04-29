using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route);

        Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint);

        MessageContext ReadMetadata(object message);

        object WriteMetadataAndContent(MessageContext context, bool useclaimcheck);

        object Deserialize(string content, Type contenttype);

        TContent Deserialize<TContent>(string content);

        string Serialize(object content);
    }
}