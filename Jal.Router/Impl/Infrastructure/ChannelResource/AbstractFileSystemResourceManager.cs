using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractFileSystemResourceManager : AbstractResourceManager
    {
        protected readonly FileSystemParameter _parameter;

        protected readonly IFileSystemTransport _transport;

        protected AbstractFileSystemResourceManager(IParameterProvider provider, IFileSystemTransport transport)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }
    }
}