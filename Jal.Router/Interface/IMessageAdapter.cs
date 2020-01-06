using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadMetadataAndContentFromPhysicalMessage(object message, Type contenttype, bool useclaimcheck);

        MessageContext ReadMetadataFromPhysicalMessage(object message);

        Task<object> WritePhysicalMessage(MessageContext context);
    }
}