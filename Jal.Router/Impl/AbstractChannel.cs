using Jal.Router.Interface;

namespace Jal.Router.Impl
{

    public abstract class AbstractChannel
    {
        protected readonly IComponentFactoryGateway Factory;

        protected readonly ILogger Logger;

        protected AbstractChannel(IComponentFactoryGateway factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}