using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Patterns
{
    public interface IMessageHandlerWithData<in TMessage, in TData> : IMessageHandler<TMessage>
    {
        Task HandleWithData(TMessage message, TData data);

        Task HandleWithContextAndData(TMessage message, MessageContext context, TData data);

        Task CompensateWithContextAndData(TMessage message, MessageContext context, TData data);

        Task CompensateWithData(TMessage message, TData data);
    }
}