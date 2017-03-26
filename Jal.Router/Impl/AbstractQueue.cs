using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractQueue : IQueue
    {
        public static IQueue Instance = new NullQueue();
        public virtual void Enqueue<TContent>(OutboundMessageContext<TContent> context)
        {

        }
    }
}