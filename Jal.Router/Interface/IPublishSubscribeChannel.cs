using Jal.Router.Model.Outbount;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send<TContent>(OutboundMessageContext<TContent> context);
    }
}