using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMessageMetadataAdapter : IMessageMetadataAdapter
    {
        public MessageContext Create<TMessage>(TMessage message)
        {
            return new MessageContext();
        }

        public TMessage Create<TMessage>(MessageContext messagecontext, TMessage message)
        {
            return default(TMessage);
        }
    }
}