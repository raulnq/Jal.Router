using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter
    {
        Task<MessageContext> ReadFromPhysicalMessage(object message, ListenerContext listener);

        Task<MessageContext> ReadFromPhysicalMessage(object message, SenderContext sender);

        Task<object> WritePhysicalMessage(MessageContext context, SenderContext sender);
    }
}