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
                if (await listenercontext.Close().ConfigureAwait(false))
                {
                    Logger.Log($"Shutdown {listenercontext.ToString()}");
                }

                if (listenercontext.Open())
                {
                    Logger.Log($"Listening {listenercontext.ToString()}");
                }
            }

            Logger.Log($"Listeners restarted");
        }
    }
}