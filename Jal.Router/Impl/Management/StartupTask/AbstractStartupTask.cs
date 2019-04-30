using Jal.Router.Interface;

namespace Jal.Router.Impl.StartupTask
{
    public abstract class AbstractStartupTask
    {
        protected readonly IComponentFactoryGateway Factory;

        protected readonly ILogger Logger;

        protected AbstractStartupTask(IComponentFactoryGateway factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}