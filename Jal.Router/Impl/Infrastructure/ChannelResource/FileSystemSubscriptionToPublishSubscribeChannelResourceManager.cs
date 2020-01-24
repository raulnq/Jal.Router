using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemSubscriptionToPublishSubscribeChannelResourceManager : AbstractFileSystemChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>
    {
        public FileSystemSubscriptionToPublishSubscribeChannelResourceManager(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}