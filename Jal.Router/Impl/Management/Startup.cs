using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Startup : IStartup
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;
        public Startup(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
        }


        public void Start()
        {
            foreach (var type in _configuration.StartupTaskTypes)
            {
                try
                {
                    var task = _factory.Create<IStartupTask>(type);

                    task.Run();
                }
                catch (Exception ex)
                {
                    _logger.Log($"Startup exception {ex}");

                    throw;
                }
            }

        }
    }
}