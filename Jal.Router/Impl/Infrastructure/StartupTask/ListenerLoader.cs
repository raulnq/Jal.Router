using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IListenerContextLoader _loader;

        public ListenerLoader(IComponentFactoryGateway factory, ILogger logger, IListenerContextLoader loader)
            :base(factory, logger)
        {
            _loader = loader;
        }

        public Task Run()
        {
            Logger.Log("Loading listeners");

            foreach (var route in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in route.Channels)
                {
                    var listenercontext = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (listenercontext != null)
                    {
                        listenercontext.Routes.Add(route);
                    }
                    else
                    {
                        _loader.Load(route, channel);
                    }
                }
            }

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }
    }
}