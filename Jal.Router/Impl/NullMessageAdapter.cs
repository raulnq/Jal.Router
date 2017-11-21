using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{
    public class NullMessageAdapter : IMessageAdapter
    {
        public InboundMessageContext<TContent> Read<TContent, TMessage>(TMessage message)
        {
            return new InboundMessageContext<TContent>(new MessageContext(), default(TContent));
        }

        public TMessage Write<TContent, TMessage>(OutboundMessageContext<TContent> context)
        {
            return default(TMessage);
        }
    }
}