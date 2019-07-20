using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route);

        Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint);

        MessageContext ReadMetadata(object message);

        Task<object> WriteMetadataAndContent(MessageContext context, EndPoint enpdoint);
    }
}