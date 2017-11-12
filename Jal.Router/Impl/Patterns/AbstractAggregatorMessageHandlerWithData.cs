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

        public abstract TAggregatedMessage CreateAgrgegatedMessage(TMessage message, MessageContext context, TData data);


        public override void Handle(TMessage message, MessageContext context, TData data)
        {
            try
            {
                OnEntry(message, context, data);

                Add(message, context, data);

                if (IsCompleted(message, context, data))
                {
                    var m = CreateAgrgegatedMessage(message, context, data);

                    _bus.Send(m, CreateOptions(message, context, data));
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