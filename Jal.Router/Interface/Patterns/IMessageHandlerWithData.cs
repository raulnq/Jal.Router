using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IMessageHandlerWithData<in TMessage, in TData> : IMessageHandler<TMessage>
    {
        void HandleWithData(TMessage message, TData data);

        void HandleWithContextAndData(TMessage message, MessageContext context, TData data);

        void CompensateWithContextAndData(TMessage message, MessageContext context, TData data);

        void CompensateWithData(TMessage message, TData data);
    }
}