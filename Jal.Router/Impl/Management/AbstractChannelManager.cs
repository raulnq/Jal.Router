using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public virtual bool CreateIfNotExistSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, SubscriptionToPublishSubscribeChannelRule rule)
        {
            return false;
        }

        public virtual bool CreateIfNotExistPublishSubscribeChannel(string connectionstring, string path)
        {
            return false;
        }

        public virtual bool CreateIfNotExistPointToPointChannel(string connectionstring, string path)
        {
            return false;
        }

        public virtual PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string path)
        {
            return null;
        }

        public virtual PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string path)
        {
            return null;
        }

        public virtual SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path,
            string subscription)
        {
            return null;
        }
    }
}