using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractPublishSubscribeChannel : AbstractChannel, IPublishSubscribeChannel
    {
        protected AbstractPublishSubscribeChannel(IComponentFactory factory, IConfiguration configuration)
            :base("publish subscriber", factory, configuration)
        {

        }
    }
}