using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.StartupTask
{

    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        public ListenerLoader(IComponentFactoryGateway factory,  IRouterConfigurationSource[] sources, ILogger logger)
            :base(factory, logger)
        {

        }

        public Task Run()
        {
            Logger.Log("Loading listeners");

            Create();

            Update();

            OpenAndListen();

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }

        private void OpenAndListen()
        {
            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                var listenerchannel = Factory.CreateListenerChannel(listenercontext.Channel.Type);

                if(listenerchannel!=null)
                {
                    listenercontext.UpdateListenerChannel(listenerchannel);

                    listenerchannel.Open(listenercontext);

                    listenerchannel.Listen(listenercontext);

                    Logger.Log($"Listening {listenercontext.Id}");
                }
            }
        }

        private void Update()
        {
            foreach (var partition in Factory.Configuration.Runtime.Partitions)
            {
                var listener = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == partition.Channel.Id);

                if (listener != null)
                {
                    listener.UpdatePartition(partition);
                }
            }
        }

        private void Create()
        {
            foreach (var item in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in item.Channels)
                {
                    var listener = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (listener != null)
                    {
                        listener.Routes.Add(item);
                    }
                    else
                    {
                        var newlistener = new ListenerContext(channel);

                        newlistener.Routes.Add(item);

                        Factory.Configuration.Runtime.ListenerContexts.Add(newlistener);
                    }
                }
            }
        }
    }
}