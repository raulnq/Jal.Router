using Jal.Router.Model.Outbount;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        void Send<TContent>(OutboundMessageContext<TContent> context);
    }
}