using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        public virtual void Handle(TMessage message)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Handle(TMessage message, dynamic context)
        {
            Handle(message);
        }
    }
}
