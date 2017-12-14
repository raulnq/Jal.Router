using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMessageAdapter : IMessageAdapter
    {
        public MessageContext<TContent> Read<TContent, TMessage>(TMessage message)
        {
            return new MessageContext<TContent>(new MessageContext(), default(TContent));
        }

        public TMessage Write<TContent, TMessage>(MessageContext<TContent> context)
        {
            return default(TMessage);
        }
    }
}