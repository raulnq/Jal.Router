using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageMetadataAdapter
    {
        MessageContext Create<TMessage>(TMessage message);

        TMessage Create<TMessage>(MessageContext messagecontext, TMessage message);
    }
}