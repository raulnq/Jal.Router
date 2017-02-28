using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageSender<TMessage> : IMessageSender<TMessage>
    {
        public virtual void Send(TMessage message)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Send(TMessage message,dynamic context)
        {
            Send(message);
        }
    }
}
