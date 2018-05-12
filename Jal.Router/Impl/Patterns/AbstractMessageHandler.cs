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

        public virtual void HandleWithContext(TMessage message, MessageContext context)
        {

        }

        public virtual bool IsSuccessfulWithContext(TMessage message, MessageContext context)
        {
            return true;
        }

        public virtual bool IsSuccessful(TMessage message)
        {
            return true;
        }

        public virtual void CompensateWithContext(TMessage message, MessageContext context)
        {
            
        }

        public virtual void Compensate(TMessage message)
        {

        }

        public virtual void OnEntry(TMessage message, MessageContext context)
        {

        }

        public virtual void OnExit(TMessage message, MessageContext context)
        {

        }

        public virtual void OnSuccess(TMessage message, MessageContext context)
        {

        }

        public virtual void OnException(TMessage message, MessageContext context, Exception ex)
        {

        }
    }
}