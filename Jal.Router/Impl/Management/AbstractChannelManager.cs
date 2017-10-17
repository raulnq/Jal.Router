using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public abstract class AbstractChannelManager : IChannelManager
    {
        public void CreateSubscriptionToPublishSubscribeChannel(string connectionstring, string path, string name, string origin)
        {
            
        }

        public void CreatePublishSubscribeChannel(string connectionstring, string name)
        {

        }

        public void CreatePointToPointChannel(string connectionstring, string name)
        {

        }
    }
}