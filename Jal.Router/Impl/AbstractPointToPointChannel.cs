using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractPointToPointChannel : AbstractChannel, IPointToPointChannel
    {

        protected AbstractPointToPointChannel(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
            :base("point to point", factory, configuration, builder)
        {

        }
    }
}