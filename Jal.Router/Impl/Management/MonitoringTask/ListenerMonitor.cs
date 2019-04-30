using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class ListenerMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerMonitor(IComponentFactoryGateway factory, ILogger logger)
            : base(factory, logger)
        {
        }

        public async Task Run(DateTime datetime)
        {
            Logger.Log($"Checking listeners");

            foreach (var metadata in Factory.Configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Listener!=null && !metadata.Listener.IsActive())
                {
                    await metadata.Listener.Close().ConfigureAwait(false);

                    Logger.Log($"Shutdown {metadata.Signature()}");

                    metadata.Listener.Open(metadata);

                    metadata.Listener.Listen();

                    Logger.Log($"Listening {metadata.Signature()}");
                }
            }

            Logger.Log($"Listeners checked");
        }
    }
}