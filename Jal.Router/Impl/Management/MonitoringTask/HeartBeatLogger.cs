using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class HeartBeatLogger : AbstractMonitoringTask, IMonitoringTask
    {

        public HeartBeatLogger( IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {
        }

        public void Run(DateTime datetime)
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<Beat>>(x)).ToArray();

                var message = new Beat() { Name = Configuration.ApplicationName, Action = "Running" };

                Array.ForEach(loggers, x => x.Log(message, datetime));
            }
        }
    }
}