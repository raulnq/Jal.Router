using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractConsumerMiddleware : AbstractMiddleware
    {
        protected AbstractConsumerMiddleware(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}