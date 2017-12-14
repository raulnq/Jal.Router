using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IChannelManager
    {
        void CreateSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, string origin);

        void CreatePublishSubscribeChannel(string connectionstring, string path);

        void CreatePointToPointChannel(string connectionstring, string path);

        PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string path);

        PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string path);

        SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription);
    }
}