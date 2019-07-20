using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ShutdownTask : IShutdownTask
    {
        private readonly IComponentFactoryGateway _factory;

        public ShutdownTask(IComponentFactoryGateway factory)
        {
            _factory = factory;
        }

        public Task Run()
        {
            if (_factory.Configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = _factory.Configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => _factory.CreateLogger<Beat>(x)).ToArray();

                var message = new Beat(_factory.Configuration.ApplicationName, "Stopped");

                var datetime = DateTime.UtcNow;

                Array.ForEach(loggers, x => x.Log(message, datetime));
            }

            return Task.CompletedTask;
        }
    }
}