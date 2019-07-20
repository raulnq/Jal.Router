using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractMonitoringTask
    {
        protected readonly IComponentFactoryGateway Factory;

        protected readonly ILogger Logger;

        protected AbstractMonitoringTask(IComponentFactoryGateway factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}