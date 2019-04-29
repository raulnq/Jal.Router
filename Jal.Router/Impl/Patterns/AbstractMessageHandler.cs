using System;
using System.Threading.Tasks;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractMessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        public virtual Task Handle(TMessage message)
        {
            return Task.CompletedTask;
        }

        public virtual Task HandleWithContext(TMessage message, MessageContext context)
        {
            return Task.CompletedTask;
        }

        public virtual bool IsSuccessfulWithContext(TMessage message, MessageContext context)
        {
            return true;
        }

        public virtual bool IsSuccessful(TMessage message)
        {
            return true;
        }

        public virtual Task CompensateWithContext(TMessage message, MessageContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task Compensate(TMessage message)
        {
            return Task.CompletedTask;
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