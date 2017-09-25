using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IMessageAdapter<T>
    {
        TContent ReadContent<TContent>(T message);

        InboundMessageContext ReadContext(T message);

        T Write<TContent>(OutboundMessageContext<TContent> message);
    }
}