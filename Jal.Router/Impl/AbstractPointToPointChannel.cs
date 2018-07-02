using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractPointToPointChannel : AbstractChannel, IPointToPointChannel
    {

        protected AbstractPointToPointChannel(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            :base("point to point", factory, configuration, logger)
        {

        }
    }
}