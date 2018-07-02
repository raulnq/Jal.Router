using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IChannelManager
    {
        bool CreateIfNotExistSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, SubscriptionToPublishSubscribeChannelRule rule);

        bool CreateIfNotExistPublishSubscribeChannel(string connectionstring, string path);

        bool CreateIfNotExistPointToPointChannel(string connectionstring, string path);

        PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string path);

        PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string path);

        SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription);
    }
}