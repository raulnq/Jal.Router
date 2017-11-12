using System;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractMessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        public virtual void Handle(TMessage message)
        {
            
        }

        public virtual void Handle(TMessage message, MessageContext context)
        {

        }

        public virtual bool IsSuccessful(TMessage message, MessageContext context)
        {
            return true;
        }

        public virtual bool IsSuccessful(TMessage message)
        {
            return true;
        }

        public virtual void Compensate(TMessage message, MessageContext context)
        {
            
        }

        public virtual void Compensate(TMessage message)
        {

        }

        public virtual void OnEntry(TMessage message)
        {

        }

        public virtual void OnExit(TMessage message)
        {

        }

        public virtual void OnException(TMessage message, Exception ex)
        {

        }

        public virtual Options CreateOptions(TMessage message)
        {
            return new Options();
        }

    }
}