using Jal.Router.Interface.Inbound;

namespace Jal.Router.Impl
{
    public class NullMessageBodyAdapter : IMessageBodyAdapter
    {
        public TContent Read<TContent, TMessage>(TMessage message)
        {
            return default(TContent);
        }

        public TMessage Write<TContent, TMessage>(TContent content)
        {
            return default(TMessage);
        }
    }
}