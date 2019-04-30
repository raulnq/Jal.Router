using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

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

            AssignGroup();

            OpenAndListen();

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }

        private void OpenAndListen()
        {
            foreach (var metadata in Factory.Configuration.Runtime.ListenersMetadata)
            {
                var listenerchannel = Factory.CreateListenerChannel(metadata.Channel.Type);

                if(listenerchannel!=null)
                {
                    metadata.Listener = listenerchannel;

                    listenerchannel.Open(metadata);

                    listenerchannel.Listen();

                    Logger.Log($"Listening {metadata.Signature()}");
                }
            }
        }

        private void AssignGroup()
        {
            foreach (var group in Factory.Configuration.Runtime.Groups)
            {
                var listener = Factory.Configuration.Runtime.ListenersMetadata.FirstOrDefault(x => x.Channel.GetId() == group.Channel.GetId());

                if (listener != null)
                {
                    listener.Group = group;
                }
            }
        }

        private void Create()
        {
            foreach (var item in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in item.Channels)
                {
                    var listener = Factory.Configuration.Runtime.ListenersMetadata.FirstOrDefault(x => x.Channel.GetId() == channel.GetId());

                    if (listener != null)
                    {
                        listener.Routes.Add(item);
                    }
                    else
                    {
                        var newlistener = new ListenerMetadata(channel);

                        newlistener.Routes.Add(item);

                        Factory.Configuration.Runtime.ListenersMetadata.Add(newlistener);
                    }
                }
            }
        }
    }
}