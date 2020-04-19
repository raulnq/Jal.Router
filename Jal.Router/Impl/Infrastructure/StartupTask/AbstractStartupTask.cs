using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractStartupTask
    {
        protected readonly IComponentFactoryFacade Factory;

        protected readonly ILogger Logger;

        protected AbstractStartupTask(IComponentFactoryFacade factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}