using System;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractSplitterMessageHandler<TMessage, TSplittedMessage> : AbstractMessageHandler<TMessage>
    {
        public abstract TSplittedMessage[] CreateSplittedMessages(TMessage message, MessageContext context);

        public abstract void Send(TSplittedMessage message, MessageContext context);

        public override void HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {

                OnEntry(message, context);

                var messages = CreateSplittedMessages(message, context);

                foreach (var m in messages)
                {
                    Send(m, context);
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