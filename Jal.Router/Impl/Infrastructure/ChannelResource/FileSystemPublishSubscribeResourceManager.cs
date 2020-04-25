using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPublishSubscribeResourceManager : AbstractFileSystemResourceManager
    {
        public FileSystemPublishSubscribeResourceManager(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(Resource resource)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(Resource resource)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}