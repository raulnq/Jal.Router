using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IListenerContextCreator _creator;

        public ListenerLoader(IComponentFactoryGateway factory, ILogger logger, IListenerContextCreator creator)
            :base(factory, logger)
        {
            _creator = creator;
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
                _creator.Open(listenercontext);
            }
        }

        private void Create()
        {
            foreach (var route in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in route.Channels)
                {
                    var listenercontext = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (listenercontext == null)
                    {
                        listenercontext = _creator.Create(channel);

                        Factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);
                    }

                    listenercontext.Routes.Add(route);
                }
            }
        }
    }
}