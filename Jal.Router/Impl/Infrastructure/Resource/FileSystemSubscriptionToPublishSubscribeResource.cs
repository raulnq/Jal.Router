using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemSubscriptionToPublishSubscribeResource : AbstractFileSystemResource
    {
        public FileSystemSubscriptionToPublishSubscribeResource(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(ResourceContext context)
        {
            var resource = context.Resource;

            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path, resource.Subscription);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(ResourceContext context)
        {
            var resource = context.Resource;

            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, resource.ConnectionString, resource.Path, resource.Subscription);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}