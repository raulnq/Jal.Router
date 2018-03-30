using System;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractAggregatorMessageHandler<TMessage, TAggregatedMessage> : AbstractMessageHandler<TMessage>
    {

        public abstract void Add(TMessage message, MessageContext context);

        public abstract bool IsCompleted(TMessage message, MessageContext context);

        public abstract void Publish(TAggregatedMessage message, MessageContext context);

        public abstract TAggregatedMessage CreateAggregatedMessage(TMessage message, MessageContext context);

        public override void HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {
                OnEntry(message, context);

                Add(message, context);

                if (IsCompleted(message, context))
                {
                    var m = CreateAggregatedMessage(message, context);

                    Publish(m, context);
                }
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