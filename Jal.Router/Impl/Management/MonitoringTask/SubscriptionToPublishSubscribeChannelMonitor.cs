using System;
using System.Linq;
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

        public void Run(DateTime datetime)
        {
            if (Configuration.LoggerTypes.ContainsKey(typeof(SubscriptionToPublishSubscribeChannelInfo)))
            {
                var loggertypes = Configuration.LoggerTypes[typeof(SubscriptionToPublishSubscribeChannelInfo)];

                var loggers = loggertypes.Select(x => Factory.Create<ILogger<SubscriptionToPublishSubscribeChannelInfo>>(x)).ToArray();

                var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

                foreach (var subscription in Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
                {
                    var message = manager.GetInfo(subscription);

                    if (message != null)
                    {
                        Array.ForEach(loggers, x => x.Log(message, datetime));
                    }
                }
            }
        }
    }
}