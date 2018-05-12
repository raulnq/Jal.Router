using System;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractMessageHandlerWithData<TMessage, TData> : AbstractMessageHandler<TMessage>, IMessageHandlerWithData<TMessage, TData>
    {
        public virtual void HandleWithData(TMessage message, TData data)
        {

        }

        public virtual void HandleWithContextAndData(TMessage message, MessageContext context, TData data)
        {

        }

        public virtual void CompensateWithContextAndData(TMessage message, MessageContext context, TData data)
        {

        }

        public virtual void CompensateWithData(TMessage message, TData data)
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

        public virtual void OnSuccess(TMessage message, MessageContext context, TData data)
        {
            OnSuccess(message, context);
        }

        public virtual void OnException(TMessage message, MessageContext context, Exception ex, TData data)
        {
            OnException(message, context, ex);
        }
    }
}