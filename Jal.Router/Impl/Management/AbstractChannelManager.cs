using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public virtual void CreateSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string subscription, string origin)
        {
            
        }

        public virtual void CreatePublishSubscribeChannel(string connectionstring, string path)
        {

        }

        public virtual void CreatePointToPointChannel(string connectionstring, string path)
        {

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