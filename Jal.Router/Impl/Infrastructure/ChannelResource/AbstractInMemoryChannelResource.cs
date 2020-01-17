using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class AbstractInMemoryChannelResource<T, S> : AbstractChannelResource<T, S>
    {
        protected readonly IInMemoryTransport _transport;

        public AbstractInMemoryChannelResource(IInMemoryTransport transport)
        {
            _transport = transport;
        }
    }
}