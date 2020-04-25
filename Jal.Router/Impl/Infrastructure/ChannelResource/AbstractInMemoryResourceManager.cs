using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractInMemoryResourceManager : AbstractResourceManager
    {
        protected readonly IInMemoryTransport _transport;

        protected AbstractInMemoryResourceManager(IInMemoryTransport transport)
        {
            _transport = transport;
        }
    }
}