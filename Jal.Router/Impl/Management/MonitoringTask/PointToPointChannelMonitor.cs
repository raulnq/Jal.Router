using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class PointToPointChannelMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public PointToPointChannelMonitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            :base(factory, configuration, logger)
        {

        }

        public async Task Run(DateTime datetime)
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof (PointToPointChannelStatistics)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof (PointToPointChannelStatistics)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<PointToPointChannelStatistics>>(x)).ToArray();

                var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

                foreach (var channel in Configuration.Runtime.PointToPointChannels)
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