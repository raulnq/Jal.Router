using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Shutdown : IShutdown
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public Shutdown(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Stop()
        {
            foreach (var type in _configuration.ShutdownTaskTypes)
            {
                try
                {
                    var task = _factory.Create<IShutdownTask>(type);

                    await task.Run().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.Log($"Shutdown exception {ex}");

                    throw;
                }

            }
        }
    }
}