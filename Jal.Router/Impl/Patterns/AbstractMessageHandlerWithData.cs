using System;
using System.Threading.Tasks;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractMessageHandlerWithData<TMessage, TData> : AbstractMessageHandler<TMessage>, IMessageHandlerWithData<TMessage, TData>
    {
        public virtual Task HandleWithData(TMessage message, TData data)
        {
            return Task.CompletedTask;
        }

        public virtual Task HandleWithContextAndData(TMessage message, MessageContext context, TData data)
        {
            return Task.CompletedTask;
        }

        public virtual Task CompensateWithContextAndData(TMessage message, MessageContext context, TData data)
        {
            return Task.CompletedTask;
        }

        public virtual Task CompensateWithData(TMessage message, TData data)
        {
            return Task.CompletedTask;
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