using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public abstract class AbstractMonitoringTask
    {
        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        protected AbstractMonitoringTask(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }
    }
}