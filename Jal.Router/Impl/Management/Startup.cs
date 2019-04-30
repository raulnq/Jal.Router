using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Startup : IStartup
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public Startup(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }


        public async Task Start()
        {
            foreach (var type in _factory.Configuration.StartupTaskTypes)
            {
                try
                {
                    var task = _factory.CreateStartupTask(type);

                    await task.Run().ConfigureAwait(false);
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