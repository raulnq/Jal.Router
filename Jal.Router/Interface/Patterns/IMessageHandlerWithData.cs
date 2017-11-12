using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IMessageHandlerWithData<in TMessage, in TData> : IMessageHandler<TMessage>
    {
        void Handle(TMessage message, TData data);

        void Handle(TMessage message, MessageContext context, TData data);

        void Compensate(TMessage message, MessageContext context, TData data);

        void Compensate(TMessage message, TData data);
    }
}