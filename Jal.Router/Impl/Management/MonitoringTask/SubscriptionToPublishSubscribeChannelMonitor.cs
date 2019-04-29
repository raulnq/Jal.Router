using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class SubscriptionToPublishSubscribeChannelMonitor : AbstractMonitoringTask, IMonitoringTask
    {

        public SubscriptionToPublishSubscribeChannelMonitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            :base(factory, configuration, logger)
        {
        }

        public async Task Run(DateTime datetime)
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof(SubscriptionToPublishSubscribeChannelStatistics)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof(SubscriptionToPublishSubscribeChannelStatistics)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<SubscriptionToPublishSubscribeChannelStatistics>>(x)).ToArray();

                var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

                foreach (var subscription in Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
                {
                    var message = await manager.Get(subscription).ConfigureAwait(false);

                    if (message != null)
                    {
                        Array.ForEach(loggers, x => x.Log(message, datetime));
                    }
                }
            }
        }
    }
}