using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractConsumerMiddleware : AbstractMessageHandler
    {
        protected AbstractConsumerMiddleware(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}