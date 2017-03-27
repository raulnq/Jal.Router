using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractPublisher : IPublisher
    {
        public static IPublisher Instance = new NullPublisher();
        public void Publish<TContent>(OutboundMessageContext<TContent> context)
        {

        }
    }
}