using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public abstract class AbstractSplitterMessageHandlerWithData<TMessage, TSplittedMessage, TData> : AbstractMessageHandlerWithData<TMessage, TData>
    {
        public abstract TSplittedMessage[] CreateSplittedMessages(TMessage message, MessageContext context, TData data);

        public abstract Task Send(TSplittedMessage message, MessageContext context, TData data, int messages);

        public virtual void OnNoSplittedMessages(TMessage message, MessageContext context, TData data)
        {

        }

        public override async Task HandleWithContextAndData(TMessage message, MessageContext context, TData data)
        {
            try
            {

                OnEntry(message, context, data);

                var messages = CreateSplittedMessages(message, context, data);

                if (messages.Length > 0)
                {
                    var counter = messages.Length;

                    foreach (var m in messages)
                    {
                        await Send(m, context, data, counter).ConfigureAwait(false);
                    }
                }
                else
                {
                    OnNoSplittedMessages(message, context, data);
                }

                OnSuccess(message, context, data);
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