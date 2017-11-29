using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);

        void HandleWithContext(TMessage message, MessageContext context);

        bool IsSuccessfulWithContext(TMessage message, MessageContext context);

        bool IsSuccessful(TMessage message);

        void CompensateWithContext(TMessage message, MessageContext context);

        void Compensate(TMessage message);
    }
}