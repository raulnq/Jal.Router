using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public abstract class AbstractStartupTask
    {
        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        protected AbstractStartupTask(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }
    }
}