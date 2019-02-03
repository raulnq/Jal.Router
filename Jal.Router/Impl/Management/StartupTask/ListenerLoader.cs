using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.StartupTask
{

    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        public ListenerLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, ILogger logger)
            :base(factory, configuration, logger)
        {

        }

        public void Run()
        {
            Logger.Log("Loading listeners");

            var pointtopointchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);

            var publishsubscribechannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);

            foreach (var item in Configuration.Runtime.Routes)
            {
                foreach (var channel in item.Channels)
                {
                    var listener = Configuration.Runtime.ListenersMetadata.FirstOrDefault(x => x.Channel.GetId() == channel.GetId());

                    if (listener != null)
                    {
                        listener.Routes.Add(item);
                    }
                    else
                    {
                        var newlistener = new ListenerMetadata(channel);

                        newlistener.Routes.Add(item);

                        Configuration.Runtime.ListenersMetadata.Add(newlistener);
                    }
                }
            }

            foreach (var group in Configuration.Runtime.Groups)
            {
                var listener = Configuration.Runtime.ListenersMetadata.FirstOrDefault(x => x.Channel.GetId() == group.Channel.GetId());

                if (listener != null)
                {
                    listener.Group = group;
                }
            }

            foreach (var metadata in Configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Channel.Type == Model.ChannelType.PointToPoint)
                {
                    metadata.CreateListenerMethod = pointtopointchannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = pointtopointchannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = pointtopointchannel.ListenerMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.Group?.ToString()} {metadata.Channel.GetPath()} {metadata.Channel.ToString()} channel ({metadata.Routes.Count}): {string.Join(",", metadata.Routes.Select(x=> x.Saga==null ? x.Name: $"{x.Saga.Name}/{x.Name}" ))}");
                }

                if (metadata.Channel.Type == Model.ChannelType.PublishSubscribe)
                {
                    metadata.CreateListenerMethod = publishsubscribechannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = publishsubscribechannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = publishsubscribechannel.ListenerMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.Group?.ToString()} {metadata.Channel.GetPath()} {metadata.Channel.ToString()} channel ({metadata.Routes.Count}): {string.Join(",", metadata.Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}");
                }
            }

            Logger.Log("Listeners loaded");
        }
    }
}