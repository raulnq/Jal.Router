using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class ListenerRestartMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerRestartMonitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
           : base(factory, configuration, logger)
        {

        }

        public async Task Run(DateTime datetime)
        {
            Logger.Log($"Restarting listeners");

            foreach (var metadata in Configuration.Runtime.ListenersMetadata)
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