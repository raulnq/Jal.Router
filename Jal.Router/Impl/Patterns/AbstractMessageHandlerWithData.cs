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

        public virtual void OnEntry(TMessage message, TData data)
        {
            OnEntry(message);
        }

        public virtual void OnExit(TMessage message, TData data)
        {
            OnExit(message);
        }

        public virtual void OnException(TMessage message, Exception ex, TData data)
        {
            OnException(message, ex);
        }

        public virtual Options CreateOptions(TMessage message, TData data)
        {
            return CreateOptions(message);
        }
    }
}