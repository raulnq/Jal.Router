using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractInMemoryResource : AbstractResource
    {
        protected readonly IInMemoryTransport _transport;

        protected AbstractInMemoryResource(IInMemoryTransport transport)
        {
            _transport = transport;
        }
    }
}