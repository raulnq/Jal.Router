using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class Startup : IStartup
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public Startup(IComponentFactoryFacade factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }


        public async Task Run()
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