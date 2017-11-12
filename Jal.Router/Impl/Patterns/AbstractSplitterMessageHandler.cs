using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractSplitterMessageHandler<TMessage, TSplittedMessage> : AbstractMessageHandler<TMessage>
    {
        private readonly IBus _bus;

        protected AbstractSplitterMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public abstract TSplittedMessage[] Split(TMessage message, MessageContext context);

        public override void Handle(TMessage message, MessageContext context)
        {
            try
            {

                OnEntry(message, context);

                var messages = Split(message, context);

                foreach (var m in messages)
                {
                    _bus.Send(m, CreateOptions(message, context));
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