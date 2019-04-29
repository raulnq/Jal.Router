using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class StartupBeatLogger : AbstractStartupTask, IStartupTask
    {
        public StartupBeatLogger(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            :base(factory, configuration, logger)
        {
        }

        public Task Run()
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<Beat>>(x)).ToArray();

                var message = new Beat() { Name = Configuration.ApplicationName, Action = "Started" };

                var startuptime = DateTime.UtcNow;

                Array.ForEach(loggers, x => x.Log(message, startuptime));
            }

            return Task.CompletedTask;
        }
    }
}