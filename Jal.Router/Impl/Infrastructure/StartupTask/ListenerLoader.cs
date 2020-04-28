using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IListenerContextLifecycle _lifecycle;

        public ListenerLoader(IComponentFactoryFacade factory, ILogger logger, IListenerContextLifecycle lifecycle)
            :base(factory, logger)
        {
            _lifecycle = lifecycle;
        }

        public Task Run()
        {
            Logger.Log("Loading listeners");

            Create();

            Open();

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }

        private void Open()
        {
            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                listenercontext.Open();
            }
        }

        private void Create()
        {
            foreach (var route in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in route.Channels)
                {
                    _lifecycle.Add(route, channel);
                }
            }
        }
    }
}