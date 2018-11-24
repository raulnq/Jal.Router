using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IChannelManager
    {
        bool CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel);

        bool CreateIfNotExist(PublishSubscribeChannel channel);

        bool CreateIfNotExist(PointToPointChannel channel);

        PublishSubscribeChannelInfo GetInfo(PublishSubscribeChannel channel);

        PointToPointChannelInfo GetInfo(PointToPointChannel channel);

        SubscriptionToPublishSubscribeChannelInfo GetInfo(SubscriptionToPublishSubscribeChannel channel);
    }
}