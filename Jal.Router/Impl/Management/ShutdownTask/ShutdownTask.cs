using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class ShutdownTask : IShutdownTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public ShutdownTask(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Run()
        {
            if (_configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = _configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => _factory.Create<ILogger<Beat>>(x)).ToArray();

                var message = new Beat() { Name = _configuration.ApplicationName, Action="Stopped" };

                var datetime = DateTime.UtcNow;

                Array.ForEach(loggers, x => x.Log(message, datetime));
            }
        }
    }
}