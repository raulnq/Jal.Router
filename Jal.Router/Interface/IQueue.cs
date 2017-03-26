using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IQueue
    {
        void Enqueue<TContent>(OutboundMessageContext<TContent> context);
    }
}