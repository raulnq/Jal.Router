using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        InboundMessageContext<TContent> Read<TContent, TMessage>(TMessage message);

        TMessage Write<TContent, TMessage>(OutboundMessageContext<TContent> context);
    }
}