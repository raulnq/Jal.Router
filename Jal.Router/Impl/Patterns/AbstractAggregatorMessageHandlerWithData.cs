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

        public abstract void Add(TMessage message, TData data);

        public abstract bool IsCompleted(TMessage message, TData data);

        public abstract TAggregatedMessage CreateAgrgegatedMessage(TMessage message, TData data);


        public override void Handle(TMessage message, MessageContext context, TData data)
        {
            try
            {
                OnEntry(message, data);

                Add(message, data);

                if (IsCompleted(message, data))
                {
                    var m = CreateAgrgegatedMessage(message, data);

                    _bus.Send(m, CreateOptions(message, data));
                }
            }
            catch (Exception ex)
            {
                OnException(message, ex);
            }
            finally
            {
                OnExit(message);
            }
        }
    }
}