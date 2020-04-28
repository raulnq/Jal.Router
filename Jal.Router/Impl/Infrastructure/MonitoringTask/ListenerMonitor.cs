using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerMonitor(IComponentFactoryFacade factory, ILogger logger)
            : base(factory, logger)
        {
        }

        public async Task Run(DateTime datetime)
        {
            Logger.Log($"Checking listeners");

            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                if (!listenercontext.IsActive())
                {
                    await listenercontext.Close().ConfigureAwait(false);

                    listenercontext.Open();
                }
            }

            Logger.Log($"Listeners checked");
        }
    }
}