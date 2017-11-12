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

        public abstract TSplittedMessage[] Split(TMessage message);

        public override void Handle(TMessage message, MessageContext context)
        {
            try
            {

                OnEntry(message);

                var messages = Split(message);

                foreach (var m in messages)
                {
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