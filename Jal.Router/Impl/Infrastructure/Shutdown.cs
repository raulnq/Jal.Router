using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class Shutdown : IShutdown
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public Shutdown(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }

        public async Task Run()
        {
            foreach (var type in _factory.Configuration.ShutdownTaskTypes)
            {
                try
                {
                    var task = _factory.CreateShutdownTask(type);

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