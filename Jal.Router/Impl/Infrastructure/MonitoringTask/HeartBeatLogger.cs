using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class HeartBeatLogger : AbstractMonitoringTask, IMonitoringTask
    {

        public HeartBeatLogger(IComponentFactoryGateway factory, ILogger logger)
            : base(factory, logger)
        {
        }

        public Task Run(DateTime datetime)
        {
            if (Factory.Configuration.LoggerTypes.ContainsKey(typeof(Beat)))
            {
                var loggertypes = Factory.Configuration.LoggerTypes[typeof(Beat)];

                var loggers = loggertypes.Select(x => Factory.CreateLogger<Beat>(x)).ToArray();

                var message = new Beat(Factory.Configuration.ApplicationName, "Running");

                Array.ForEach(loggers, x => x.Log(message, datetime));
            }

            return Task.CompletedTask;
        }
    }
}