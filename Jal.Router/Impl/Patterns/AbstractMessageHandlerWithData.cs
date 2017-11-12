using System;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractMessageHandlerWithData<TMessage, TData> : AbstractMessageHandler<TMessage>
    {
        public virtual void Handle(TMessage message, TData data)
        {

        }

        public virtual void Handle(TMessage message, MessageContext context, TData data)
        {

        }

        public virtual void Compensate(TMessage message, MessageContext context, TData data)
        {

        }

        public virtual void Compensate(TMessage message, TData data)
        {

        }

        public virtual void OnEntry(TMessage message, MessageContext context, TData data)
        {
            OnEntry(message, context);
        }

        public virtual void OnExit(TMessage message, MessageContext context, TData data)
        {
            OnExit(message, context);
        }

        public virtual void OnException(TMessage message, MessageContext context, Exception ex, TData data)
        {
            OnException(message, context, ex);
        }

        public virtual Options CreateOptions(TMessage message, MessageContext context, TData data)
        {
            return CreateOptions(message, context);
        }
    }
}