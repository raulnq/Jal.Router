using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        MessageContext<TContent> Read<TContent, TMessage>(TMessage message);

        TMessage Write<TContent, TMessage>(MessageContext<TContent> context);
    }
}