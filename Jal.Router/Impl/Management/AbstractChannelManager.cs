using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;
using System.Threading.Tasks;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public virtual Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> CreateIfNotExist(PublishSubscribeChannel channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> CreateIfNotExist(PointToPointChannel channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannel channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PublishSubscribeChannel channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PointToPointChannel channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannel channel)
        {
            return Task.FromResult(default(PublishSubscribeChannelStatistics));
        }

        public virtual Task<PointToPointChannelStatistics> Get(PointToPointChannel channel)
        {
            return Task.FromResult(default(PointToPointChannelStatistics));
        }

        public virtual Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannel channel)
        {
            return Task.FromResult(default(SubscriptionToPublishSubscribeChannelStatistics));
        }
    }
}