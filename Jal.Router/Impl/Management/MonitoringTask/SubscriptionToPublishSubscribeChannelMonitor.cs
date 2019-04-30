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

        public SubscriptionToPublishSubscribeChannelMonitor(IComponentFactoryGateway factory, ILogger logger)
            :base(factory, logger)
        {
        }

        public async Task Run(DateTime datetime)
        {
            if (Factory.Configuration.LoggerTypes.ContainsKey(typeof(SubscriptionToPublishSubscribeChannelStatistics)))
            {
                var loggertypes = Factory.Configuration.LoggerTypes[typeof(SubscriptionToPublishSubscribeChannelStatistics)];

                var loggers = loggertypes.Select(x => Factory.CreateLogger<SubscriptionToPublishSubscribeChannelStatistics>(x)).ToArray();

                var manager = Factory.CreateChannelManager();

                foreach (var subscription in Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
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