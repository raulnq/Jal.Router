using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class NullMessageAdapter : IMessageAdapter
    {
        public Task<object> WritePhysicalMessage(MessageContext context, SenderContext sender)
        {
            return Task.FromResult(default(object));
        }

        public Task<MessageContext> ReadFromPhysicalMessage(object message, ListenerContext listener)
        {
            return Task.FromResult(default(MessageContext));
        }

        public Task<MessageContext> ReadFromPhysicalMessage(object message, SenderContext sender)
        {
            return Task.FromResult(default(MessageContext));
        }
    }
}