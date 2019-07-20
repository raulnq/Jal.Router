using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractChannelResource : IChannelResource
    {
        public virtual Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel)
        {
            return Task.FromResult(default(PublishSubscribeChannelStatistics));
        }

        public virtual Task<PointToPointChannelStatistics> Get(PointToPointChannelResource channel)
        {
            return Task.FromResult(default(PointToPointChannelStatistics));
        }

        public virtual Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(default(SubscriptionToPublishSubscribeChannelStatistics));
        }
    }
}