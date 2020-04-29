using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerRestartMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerRestartMonitor(IComponentFactoryFacade factory, ILogger logger)
           : base(factory, logger)
        {

        }

        public async Task Run(DateTime datetime)
        {
            Logger.Log($"Restarting listeners");

            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                await listenercontext.Close().ConfigureAwait(false);

                listenercontext.Open();
            }

            Logger.Log($"Listeners restarted");
        }
    }
}