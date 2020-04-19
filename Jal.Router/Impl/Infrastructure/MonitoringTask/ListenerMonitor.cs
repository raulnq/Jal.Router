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
                    if(await listenercontext.Close().ConfigureAwait(false))
                    {
                        Logger.Log($"Shutdown {listenercontext.Id}");
                    }

                    if(listenercontext.Open())
                    {
                        Logger.Log($"Listening {listenercontext.Id}");
                    }
                }
            }

            Logger.Log($"Listeners checked");
        }
    }
}