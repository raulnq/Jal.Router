using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractAggregatorMessageHandler<TMessage, TAggregatedMessage> : AbstractMessageHandler<TMessage>
    {
        private readonly IBus _bus;

        protected AbstractAggregatorMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public abstract void Add(TMessage message, MessageContext context);

        public abstract bool IsCompleted(TMessage message, MessageContext context);

        public virtual Origin CreateOrigin(TMessage message, MessageContext context)
        {
            return null;
        }

        public abstract TAggregatedMessage CreateAgrgegatedMessage(TMessage message, MessageContext context);

        public override void HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {
                OnEntry(message, context);

                Add(message, context);

                if (IsCompleted(message, context))
                {
                    var m = CreateAgrgegatedMessage(message, context);

                    var origin = CreateOrigin(message, context);

                    if (origin == null)
                    {
                        _bus.Publish(m, CreateOptions(message, context));
                    }
                    else
                    {
                        _bus.Publish(m, origin, CreateOptions(message, context));
                    }
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