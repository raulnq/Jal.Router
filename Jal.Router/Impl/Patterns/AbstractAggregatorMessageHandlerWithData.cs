using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractAggregatorMessageHandlerWithData<TMessage, TAggregatedMessage, TData> : AbstractMessageHandlerWithData<TMessage, TData>
    {
        private readonly IBus _bus;

        protected AbstractAggregatorMessageHandlerWithData(IBus bus)
        {
            _bus = bus;
        }

        public abstract void Add(TMessage message, MessageContext context, TData data);

        public abstract bool IsCompleted(TMessage message, MessageContext context, TData data);

        public abstract TAggregatedMessage CreateAggregatedMessage(TMessage message, MessageContext context, TData data);

        public virtual Origin CreateOrigin(TMessage message, MessageContext context, TData data)
        {
            return null;
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

                    var origin = CreateOrigin(message, context, data);

                    if (origin == null)
                    {
                        _bus.Publish(m, CreateOptions(message, context, data));
                    }
                    else
                    {
                        _bus.Publish(m, origin, CreateOptions(message, context, data));
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