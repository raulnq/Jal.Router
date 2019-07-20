using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class StartupBeatLogger : AbstractStartupTask, IStartupTask
    {
        public StartupBeatLogger(IComponentFactoryGateway factory, ILogger logger)
            :base(factory, logger)
        {
        }

        public Task Run()
        {
            if (Factory.Configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = Factory.Configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => Factory.CreateLogger<Beat>(x)).ToArray();

                var message = new Beat(Factory.Configuration.ApplicationName, "Started");

                var startuptime = DateTime.UtcNow;

                Array.ForEach(loggers, x => x.Log(message, startuptime));
            }

            return Task.CompletedTask;
        }
    }
}