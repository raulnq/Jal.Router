using System;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractAggregatorMessageHandlerWithData<TMessage, TAggregatedMessage, TData> : AbstractMessageHandlerWithData<TMessage, TData>
    {
        public abstract void Add(TMessage message, MessageContext context, TData data);

        public abstract bool IsCompleted(TMessage message, MessageContext context, TData data);

        public abstract TAggregatedMessage CreateAggregatedMessage(TMessage message, MessageContext context, TData data);

        public abstract void Publish(TAggregatedMessage message, MessageContext context, TData data);

        public virtual void OnNoCompleted(TMessage message, MessageContext context, TData data)
        {
            
        }

        public override void HandleWithContextAndData(TMessage message, MessageContext context, TData data)
        {
            try
            {
                OnEntry(message, context, data);

                Add(message, context, data);

                if (IsCompleted(message, context, data))
                {
                    var m = CreateAggregatedMessage(message, context, data);

                    Publish(m, context, data);
                }
                else
                {
                    OnNoCompleted(message, context, data);
                }

                OnSuccess(message, context);
            }
            catch (Exception ex)
            {
                OnException(message, context, ex);
            }
            finally
            {
                OnExit(message, context);
            }
        }
    }
}