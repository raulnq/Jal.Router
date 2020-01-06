using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class NullMessageAdapter : IMessageAdapter
    {
        public Task<object> WritePhysicalMessage(MessageContext context)
        {
            return Task.FromResult(default(object));
        }

        public MessageContext ReadMetadataFromPhysicalMessage(object message)
        {
            return default(MessageContext);
        }

        public Task<MessageContext> ReadMetadataAndContentFromPhysicalMessage(object message, Type contenttype, bool useclaimcheck)
        {
            return Task.FromResult(default(MessageContext));
        }
    }
}