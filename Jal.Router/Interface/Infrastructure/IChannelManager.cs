using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelResource
    {
        Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel);

        Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel);

        Task<bool> CreateIfNotExist(PointToPointChannelResource channel);

        Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel);

        Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel);

        Task<bool> DeleteIfExist(PointToPointChannelResource channel);

        Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel);

        Task<PointToPointChannelStatistics> Get(PointToPointChannelResource channel);

        Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannelResource channel);
    }
}