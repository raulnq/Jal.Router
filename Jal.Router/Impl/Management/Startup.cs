using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Startup : IStartup
    {
        private readonly IStartupTask[] _startupTasks;

        public Startup(IStartupTask[] startupTasks)
        {
            _startupTasks = startupTasks;
        }

        public void Start()
        {
            foreach (var configuration in _startupTasks)
            {
                configuration.Run();
            }
        }
    }
}