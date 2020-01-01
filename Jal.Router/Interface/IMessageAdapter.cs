using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadMetadataAndContentFromPhysicalMessage(object message, Route route);

        Task<MessageContext> ReadMetadataAndContentPhysicalMessage(object message, EndPoint endpoint);

        MessageContext ReadMetadataFromPhysicalMessage(object message);

        Task<object> WritePhysicalMessage(MessageContext context);
    }
}