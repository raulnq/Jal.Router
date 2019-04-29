using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{

    public abstract class AbstractChannel
    {
        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        protected AbstractChannel( IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }
    }
}