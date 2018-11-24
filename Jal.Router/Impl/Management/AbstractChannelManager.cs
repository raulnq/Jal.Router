using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public virtual bool CreateIfNotExist(SubscriptionToPublishSubscribeChannel channel)
        {
            return false;
        }

        public virtual bool CreateIfNotExist(PublishSubscribeChannel channel)
        {
            return false;
        }

        public virtual bool CreateIfNotExist(PointToPointChannel channel)
        {
            return false;
        }

        public virtual PublishSubscribeChannelInfo GetInfo(PublishSubscribeChannel channel)
        {
            return null;
        }

        public virtual PointToPointChannelInfo GetInfo(PointToPointChannel channel)
        {
            return null;
        }

        public virtual SubscriptionToPublishSubscribeChannelInfo GetInfo(SubscriptionToPublishSubscribeChannel channel)
        {
            return null;
        }
    }
}