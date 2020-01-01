using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractProducerMiddleware : AbstractMiddleware
    {
        protected AbstractProducerMiddleware(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }
    }
}