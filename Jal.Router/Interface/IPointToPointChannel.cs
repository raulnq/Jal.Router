using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        void Send<TContent>(OutboundMessageContext<TContent> context);
    }
}