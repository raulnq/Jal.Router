using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPointToPointlResource : AbstractFileSystemResource
    {
        public FileSystemPointToPointlResource(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var path = _transport.CreatePointToPointChannelPath(_parameter, resource.ConnectionString, resource.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var path = _transport.CreatePointToPointChannelPath(_parameter, resource.ConnectionString, resource.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}