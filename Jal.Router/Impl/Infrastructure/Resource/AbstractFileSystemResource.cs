using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractFileSystemResource : AbstractResource
    {
        protected readonly FileSystemParameter _parameter;

        protected readonly IFileSystemTransport _transport;

        protected AbstractFileSystemResource(IParameterProvider provider, IFileSystemTransport transport)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }
    }
}