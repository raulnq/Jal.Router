using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class PointToPointChannelMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public PointToPointChannelMonitor(IComponentFactoryGateway factory, ILogger logger)
            :base(factory, logger)
        {

        }

        public async Task Run(DateTime datetime)
        {
            if (Factory.Configuration.LoggerTypes.ContainsKey(typeof (PointToPointChannelStatistics)))
            {
                var loggertypes = Factory.Configuration.LoggerTypes[typeof (PointToPointChannelStatistics)];

                var loggers = loggertypes.Select(x => Factory.CreateLogger<PointToPointChannelStatistics>(x)).ToArray();

                var manager = Factory.CreatePointToPointChannelResource();

                foreach (var channel in Factory.Configuration.Runtime.PointToPointChannelResources)
                {
                    var message = await manager.Get(channel).ConfigureAwait(false);

                    if (message != null)
                    {
                        Array.ForEach(loggers, x=> x.Log(message, datetime));
                    }
                }
            }
        }
    }
}