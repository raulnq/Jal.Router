using System;
using System.Linq;
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

        public void Run(DateTime datetime)
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof (PointToPointChannelInfo)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof (PointToPointChannelInfo)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<PointToPointChannelInfo>>(x)).ToArray();

                var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

                foreach (var channel in Configuration.Runtime.PointToPointChannels)
                {
                    var message = manager.GetInfo(channel);

                    if (message != null)
                    {
                        Array.ForEach(loggers, x=> x.Log(message, datetime));
                    }
                }
            }
        }
    }
}