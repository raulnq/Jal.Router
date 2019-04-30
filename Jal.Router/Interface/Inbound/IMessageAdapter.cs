using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route, Channel channel, Saga saga = null);

        Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint endpoint);

        MessageContext ReadMetadata(object message);

        Task<object> WriteMetadataAndContent(MessageContext context, EndPoint enpdoint);
    }
}