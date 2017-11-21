using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public virtual void CreateSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string name, string origin)
        {
            
        }

        public virtual void CreatePublishSubscribeChannel(string connectionstring, string name)
        {

        }

        public virtual void CreatePointToPointChannel(string connectionstring, string name)
        {

        }

        public virtual PublishSubscribeChannelInfo GetPublishSubscribeChannel(string connectionstring, string name)
        {
            return null;
        }

        public virtual PointToPointChannelInfo GetPointToPointChannel(string connectionstring, string name)
        {
            return null;
        }

        public virtual SubscriptionToPublishSubscribeChannelInfo GetSubscriptionToPublishSubscribeChannel(string connectionstring, string path,
            string name)
        {
            return null;
        }
    }
}