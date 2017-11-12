using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractSplitterMessageHandlerWithData<TMessage, TSplittedMessage, TData> : AbstractMessageHandlerWithData<TMessage, TData>
    {
        private readonly IBus _bus;

        protected AbstractSplitterMessageHandlerWithData(IBus bus)
        {
            _bus = bus;
        }

        public abstract TSplittedMessage[] Split(TMessage message, MessageContext context, TData data);

        public override void Handle(TMessage message, MessageContext context, TData data)
        {
            try
            {

                OnEntry(message, context, data);

                var messages = Split(message, context, data);

                foreach (var m in messages)
                {
                    _bus.Send(m, CreateOptions(message, context, data));
                }
            }
            catch (Exception ex)
            {
                OnException(message, context, ex, data);
            }
            finally
            {
                OnExit(message, context, data);
            }
        }
    }
}