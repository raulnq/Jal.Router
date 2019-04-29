using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractSplitterMessageHandler<TMessage, TSplittedMessage> : AbstractMessageHandler<TMessage>
    {
        public abstract TSplittedMessage[] CreateSplittedMessages(TMessage message, MessageContext context);

        public virtual void OnNoSplittedMessages(TMessage message, MessageContext context)
        {
            
        }

        public abstract Task Send(TSplittedMessage message, MessageContext context, int messages);

        public override async Task HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {

                OnEntry(message, context);

                var messages = CreateSplittedMessages(message, context);

                if (messages.Length > 0)
                {
                    var counter = messages.Length;

                    foreach (var m in messages)
                    {
                        await Send(m, context, counter).ConfigureAwait(false);
                    }
                }
                else
                {
                    OnNoSplittedMessages(message, context);
                }

                OnSuccess(message, context);
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