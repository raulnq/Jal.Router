using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class AbstractFileSystemChannelResourceManager<T, S> : AbstractChannelResourceManager<T, S>
    {
        protected readonly FileSystemParameter _parameter;

        protected readonly IFileSystemTransport _transport;

        public AbstractFileSystemChannelResourceManager(IParameterProvider provider, IFileSystemTransport transport)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }
    }
}