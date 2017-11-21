using Jal.Router.Interface.Outbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send<TContent>(OutboundMessageContext<TContent> context);
    }
}