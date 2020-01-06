using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractProducerMiddleware : AbstractMiddleware
    {
        protected AbstractProducerMiddleware(IComponentFactoryGateway factory) : base(factory)
        {

        }
    }
}