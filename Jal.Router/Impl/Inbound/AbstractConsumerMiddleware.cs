using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractConsumerMiddleware : AbstractMessageHandler
    {
        protected AbstractConsumerMiddleware(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}