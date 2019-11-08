using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractProducerMessageHandler : AbstractMessageHandler
    {
        protected AbstractProducerMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}