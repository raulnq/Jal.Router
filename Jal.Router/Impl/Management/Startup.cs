using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Startup : IStartup
    {
        private readonly IStartupConfiguration[] _startupConfigurations;

        public Startup(IStartupConfiguration[] startupConfigurations)
        {
            _startupConfigurations = startupConfigurations;
        }

        public void Start()
        {
            foreach (var configuration in _startupConfigurations)
            {
                configuration.Run();
            }
        }
    }
}