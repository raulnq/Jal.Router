using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IListenerContextCreator _loader;

        public ListenerLoader(IComponentFactoryGateway factory, ILogger logger, IListenerContextCreator loader)
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

                    if (listenercontext == null)
                    {
                        listenercontext = _loader.Create(channel);

                        Factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);
                    }

                    listenercontext.Routes.Add(route);
                }
            }

            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                _loader.Open(listenercontext);
            }

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }
    }
}