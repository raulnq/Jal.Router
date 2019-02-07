using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class ListenerMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerMonitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {
        }

        public void Run(DateTime datetime)
        {
            foreach (var metadata in Configuration.Runtime.ListenersMetadata)
            {
                if (!metadata.IsActiveMethod(metadata.Listener))
                {
                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.Group?.ToString()} {metadata.Channel.GetPath()} {metadata.Channel.ToString()} channel ({metadata.Routes.Count}): {string.Join(",", metadata.Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}");
                }
            }

            Logger.Log($"Checking listeners");
        }
    }
}