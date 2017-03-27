using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPublisher
    {
        void Publish<TContent>(OutboundMessageContext<TContent> context);
    }
}