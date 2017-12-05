using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class StartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public StartupTask(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Run()
        {
            if (_configuration.LoggerTypes.ContainsKey(typeof(StartupBeat)))
            {
                var loggertypes = _configuration.LoggerTypes[typeof(StartupBeat)];

                var loggers = loggertypes.Select(x => _factory.Create<ILogger<StartupBeat>>(x)).ToArray();

                Array.ForEach(loggers, x => x.Log(new StartupBeat() { Name = _configuration.ApplicationName }, DateTime.UtcNow));
            }
        }
    }
}