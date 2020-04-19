using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractMonitoringTask
    {
        protected readonly IComponentFactoryFacade Factory;

        protected readonly ILogger Logger;

        protected AbstractMonitoringTask(IComponentFactoryFacade factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}