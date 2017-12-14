using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Shutdown : IShutdown
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Shutdown(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Stop()
        {
            foreach (var type in _configuration.ShutdownTaskTypes)
            {
                var task = _factory.Create<IShutdownTask>(type);

                task.Run();
            }
        }
    }
}