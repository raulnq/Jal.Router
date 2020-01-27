using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Patterns.Impl
{
    public abstract class AbstractAggregatorMessageHandlerWithData<TMessage, TAggregatedMessage, TData> : AbstractMessageHandlerWithData<TMessage, TData>
    {
        public abstract void Add(TMessage message, MessageContext context, TData data);

        public abstract bool IsCompleted(TMessage message, MessageContext context, TData data);

        public abstract TAggregatedMessage CreateAggregatedMessage(TMessage message, MessageContext context, TData data);

        public abstract Task Publish(TAggregatedMessage message, MessageContext context, TData data);

        public virtual void OnNoCompleted(TMessage message, MessageContext context, TData data)
        {
            
        }

        public override async Task HandleWithContextAndData(TMessage message, MessageContext context, TData data)
        {
            try
            {
                OnEntry(message, context, data);

                Add(message, context, data);

                if (IsCompleted(message, context, data))
                {
                    var m = CreateAggregatedMessage(message, context, data);

                    await Publish(m, context, data).ConfigureAwait(false);
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