using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class AbstractInMemoryChannelResourceManager<T, S> : AbstractChannelResourceManager<T, S>
    {
        protected readonly IInMemoryTransport _transport;

        public AbstractInMemoryChannelResourceManager(IInMemoryTransport transport)
        {
            _transport = transport;
        }
    }
}