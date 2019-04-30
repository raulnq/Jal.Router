using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class ListenerRestartMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerRestartMonitor(IComponentFactoryGateway factory, ILogger logger)
           : base(factory, logger)
        {

        }

        public async Task Run(DateTime datetime)
        {
            Logger.Log($"Restarting listeners");

            foreach (var metadata in Factory.Configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Listener != null)
                {
                    await metadata.Listener.Close().ConfigureAwait(false);

                    Logger.Log($"Shutdown {metadata.Signature()}");

                    metadata.Listener.Open(metadata);

                    metadata.Listener.Listen();

                    Logger.Log($"Listening {metadata.Signature()}");
                }
            }

            Logger.Log($"Listeners restarted");
        }
    }
}