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

            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                if (listenercontext.ListenerChannel != null)
                {
                    await listenercontext.ListenerChannel.Close(listenercontext).ConfigureAwait(false);

                    Logger.Log($"Shutdown {listenercontext.Id}");

                    listenercontext.ListenerChannel.Open(listenercontext);

                    listenercontext.ListenerChannel.Listen(listenercontext);

                    Logger.Log($"Listening {listenercontext.Id}");
                }
            }

            Logger.Log($"Listeners restarted");
        }
    }
}