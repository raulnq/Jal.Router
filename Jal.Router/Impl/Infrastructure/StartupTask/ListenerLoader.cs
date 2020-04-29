using System.Linq;
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

        public async Task Run()
        {
            Logger.Log("Loading listeners");

            Add();

            await Create();

            Open();

            Logger.Log("Listeners loaded");
        }

        private void Open()
        {
            foreach (var context in Factory.Configuration.Runtime.ListenerContexts)
            {
                context.Open();

                if(Factory.Configuration.Runtime.Contexts.All(x=>x.Id!=context.Id))
                {
                    Factory.Configuration.Runtime.Contexts.Add(context);
                }
            }
        }

        private async Task Create()
        {
            foreach (var context in Factory.Configuration.Runtime.ListenerContexts)
            {
                if (context.Channel.UseCreateIfNotExists)
                {
                    await context.CreateIfNotExist();
                }
            }
        }

        private void Add()
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