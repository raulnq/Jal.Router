using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Patterns.Impl
{
    public abstract class AbstractAggregatorMessageHandler<TMessage, TAggregatedMessage> : AbstractMessageHandler<TMessage>
    {

        public abstract void Add(TMessage message, MessageContext context);

        public abstract bool IsCompleted(TMessage message, MessageContext context);

        public abstract Task Publish(TAggregatedMessage message, MessageContext context);

        public abstract TAggregatedMessage CreateAggregatedMessage(TMessage message, MessageContext context);

        public virtual void OnNoCompleted(TMessage message, MessageContext context)
        {

        }

        public override async Task HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {
                OnEntry(message, context);

                Add(message, context);

                if (IsCompleted(message, context))
                {
                    var m = CreateAggregatedMessage(message, context);

                    await Publish(m, context).ConfigureAwait(false);
                }
                else
                {
                    OnNoCompleted(message, context);
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