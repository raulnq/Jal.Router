using Jal.Router.Model.Management;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IChannelManager
    {
        Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel);

        Task<bool> CreateIfNotExist(PublishSubscribeChannel channel);

        Task<bool> CreateIfNotExist(PointToPointChannel channel);

        Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannel channel);

        Task<bool> DeleteIfExist(PublishSubscribeChannel channel);

        Task<bool> DeleteIfExist(PointToPointChannel channel);

        Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannel channel);

        Task<PointToPointChannelStatistics> Get(PointToPointChannel channel);

        Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannel channel);
    }
}