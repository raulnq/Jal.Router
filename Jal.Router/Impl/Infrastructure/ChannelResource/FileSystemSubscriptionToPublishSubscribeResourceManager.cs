using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemSubscriptionToPublishSubscribeResourceManager : AbstractFileSystemResourceManager
    {
        public FileSystemSubscriptionToPublishSubscribeResourceManager(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(Resource resource)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path, resource.Subscription);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(Resource resource)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path, resource.Subscription);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}