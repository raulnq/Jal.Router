using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter<T>
    {
        InboundMessageContext<TContent> Read<TContent>(T message);

        T Write<TContent>(OutboundMessageContext<TContent> message);
    }
}