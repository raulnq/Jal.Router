using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Startup : IStartup
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Startup(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }


        public void Start()
        {
            foreach (var type in _configuration.StartupTaskTypes)
            {
                var task = _factory.Create<IStartupTask>(type);

                task.Run();
            }

        }
    }
}