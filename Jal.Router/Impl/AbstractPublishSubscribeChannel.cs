using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractPublishSubscribeChannel : AbstractChannel, IPublishSubscribeChannel
    {
        protected AbstractPublishSubscribeChannel(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
            :base("publish subscriber", factory, configuration, builder)
        {

        }
    }
}