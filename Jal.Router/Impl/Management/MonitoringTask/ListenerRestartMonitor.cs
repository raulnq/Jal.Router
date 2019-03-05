using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.MonitoringTask
{
    public class ListenerRestartMonitor : AbstractMonitoringTask, IMonitoringTask
    {
        public ListenerRestartMonitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
           : base(factory, configuration, logger)
        {

        }

        public void Run(DateTime datetime)
        {
            var pointtopointchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);

            var publishsubscribechannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);

            foreach (var listenermetadata in Configuration.Runtime.ListenersMetadata)
            {
                if (listenermetadata.DestroyListenerMethod != null)
                {
                    listenermetadata.DestroyListenerMethod(listenermetadata.Listener);

                    Logger.Log($"Shutdown {listenermetadata.Channel.GetPath()} {listenermetadata.Channel.ToString()} channel");
                }
            }

            foreach (var metadata in Configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Channel.Type == Model.ChannelType.PointToPoint)
                {
                    metadata.CreateListenerMethod = pointtopointchannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = pointtopointchannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = pointtopointchannel.ListenerMethodFactory(metadata);

                    metadata.IsActiveMethod = pointtopointchannel.IsActiveMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.Group?.ToString()} {metadata.Channel.GetPath()} {metadata.Channel.ToString()} channel ({metadata.Routes.Count}): {string.Join(",", metadata.Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}");
                }

                if (metadata.Channel.Type == Model.ChannelType.PublishSubscribe)
                {
                    metadata.CreateListenerMethod = publishsubscribechannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = publishsubscribechannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = publishsubscribechannel.ListenerMethodFactory(metadata);

                    metadata.IsActiveMethod = publishsubscribechannel.IsActiveMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.Group?.ToString()} {metadata.Channel.GetPath()} {metadata.Channel.ToString()} channel ({metadata.Routes.Count}): {string.Join(",", metadata.Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}");
                }
            }
        }
    }
}