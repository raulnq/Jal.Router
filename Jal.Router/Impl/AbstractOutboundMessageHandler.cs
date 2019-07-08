using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractOutboundMessageHandler : AbstractMessageHandler
    {
        protected AbstractOutboundMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}