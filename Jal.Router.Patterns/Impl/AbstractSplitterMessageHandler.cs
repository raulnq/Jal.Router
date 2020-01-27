using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Patterns.Impl
{
    public abstract class AbstractSplitterMessageHandler<TMessage, TSplittedMessage> : AbstractMessageHandler<TMessage>
    {
        public abstract TSplittedMessage[] CreateSplittedMessages(TMessage message, MessageContext context);

        public virtual void OnNoSplittedMessages(TMessage message, MessageContext context)
        {
            
        }

        public abstract Task Send(TSplittedMessage message, MessageContext context, int length, int index);

        public override async Task HandleWithContext(TMessage message, MessageContext context)
        {
            try
            {

                OnEntry(message, context);

                var messages = CreateSplittedMessages(message, context);

                if (messages.Length > 0)
                {
                    var length = messages.Length;
                    var index = 0;
                    foreach (var m in messages)
                    {
                        await Send(m, context, length, index).ConfigureAwait(false);
                        index++;
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