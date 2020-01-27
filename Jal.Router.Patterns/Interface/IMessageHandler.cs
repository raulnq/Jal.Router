using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Patterns.Interface
{
    public interface IMessageHandler<in TMessage>
    {
        Task Handle(TMessage message);

        Task HandleWithContext(TMessage message, MessageContext context);

        bool IsSuccessfulWithContext(TMessage message, MessageContext context);

        bool IsSuccessful(TMessage message);

        Task CompensateWithContext(TMessage message, MessageContext context);

        Task Compensate(TMessage message);
    }
}