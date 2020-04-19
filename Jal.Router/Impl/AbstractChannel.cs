using Jal.Router.Interface;

namespace Jal.Router.Impl
{

    public abstract class AbstractChannel
    {
        protected readonly IComponentFactoryFacade Factory;

        protected readonly ILogger Logger;

        protected AbstractChannel(IComponentFactoryFacade factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}