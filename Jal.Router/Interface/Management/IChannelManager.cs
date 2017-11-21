using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IChannelManager
    {
        void CreateSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string name, string origin);

        void CreatePublishSubscribeChannel(string connectionstring, string name);

        void CreatePointToPointChannel(string connectionstring, string name);

        PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string name);

        PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string name);

        SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string name);
    }
}