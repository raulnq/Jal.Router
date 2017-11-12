using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);

        void Handle(TMessage message, MessageContext context);

        bool IsSuccessful(TMessage message, MessageContext context);

        bool IsSuccessful(TMessage message);

        void Compensate(TMessage message, MessageContext context);

        void Compensate(TMessage message);
    }
}