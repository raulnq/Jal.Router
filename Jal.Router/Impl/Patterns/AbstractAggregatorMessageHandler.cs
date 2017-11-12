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

        public abstract void Add(TMessage message);

        public abstract bool IsCompleted(TMessage message);

        public abstract TAggregatedMessage CreateAgrgegatedMessage(TMessage message);

        public override void Handle(TMessage message, MessageContext context)
        {
            try
            {
                OnEntry(message);

                Add(message);

                if (IsCompleted(message))
                {
                    var m = CreateAgrgegatedMessage(message);

                    _bus.Send(m, CreateOptions(message));
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