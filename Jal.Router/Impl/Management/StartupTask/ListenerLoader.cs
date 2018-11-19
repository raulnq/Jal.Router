using System.Collections.Generic;
using System.Linq;
using Jal.Router.Impl.Management;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.StartupTask
{

    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouter _router;

        private readonly ISagaExecutionCoordinator _sec;

        public ListenerLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaExecutionCoordinator sec)
            :base(factory, configuration, logger)
        {
            _router = router;
            _sec = sec;
        }

        public void Run()
        {
            Logger.Log("Loading listeners");

            var pointtopointchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);

            var publishsubscriberchannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);

            var routesmetadata = new List<RouteMetadata>();

            routesmetadata.AddRange(Configuration.Runtime.Routes.Select(route => new RouteMetadata() { Route = route, Handler = message => _router.Route(message, route), Name = route.Name }));

            foreach (var saga in Configuration.Runtime.Sagas)
            {
                if (saga.FirstRoute != null)
                {
                    routesmetadata.Add(new RouteMetadata() { Route = saga.FirstRoute, Handler = message => _sec.Start(message, saga, saga.FirstRoute), Name = $"{saga.Name}/{saga.FirstRoute.Name}" });
                }

                if (saga.LastRoute != null)
                {
                    routesmetadata.Add(new RouteMetadata() { Route = saga.LastRoute, Handler = message => _sec.End(message, saga, saga.LastRoute), Name = $"{saga.Name}/{saga.LastRoute.Name}" });
                }

                routesmetadata.AddRange(saga.Routes.Select(route => new RouteMetadata() { Route = route, Handler = message => _sec.Continue(message, saga, route), Name = $"{saga.Name}/{route.Name}" }));
            }

            foreach (var item in routesmetadata)
            {
                foreach (var channel in item.Route.Channels)
                {
                    var listener = Configuration.Runtime.ListenersMetadata.FirstOrDefault(x => x.GetId() == channel.GetId());

                    if (listener != null)
                    {
                        listener.Handlers.Add(item.Handler);

                        listener.Names.Add(item.Name);

                        listener.Routes.Add(item.Route);
                    }
                    else
                    {
                        var newlistener = new ListenerMetadata(channel.ToPath, channel.ToConnectionString, channel.ToSubscription, channel.Type);

                        newlistener.Handlers.Add(item.Handler);

                        newlistener.Names.Add(item.Name);

                        newlistener.Routes.Add(item.Route);

                        Configuration.Runtime.ListenersMetadata.Add(newlistener);
                    }
                }
            }

            foreach (var metadata in Configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Type == Model.ChannelType.PointToPoint)
                {
                    metadata.CreateListenerMethod = pointtopointchannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = pointtopointchannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = pointtopointchannel.ListenerMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.GetPath()} {metadata.ToString()} channel ({metadata.Handlers.Count}): {string.Join(",", metadata.Names)}");
                }

                if (metadata.Type == Model.ChannelType.PublishSubscriber)
                {
                    metadata.CreateListenerMethod = publishsubscriberchannel.CreateListenerMethodFactory(metadata);

                    metadata.DestroyListenerMethod = publishsubscriberchannel.DestroyListenerMethodFactory(metadata);

                    metadata.ListenMethod = publishsubscriberchannel.ListenerMethodFactory(metadata);

                    metadata.Listener = metadata.CreateListenerMethod();

                    metadata.ListenMethod(metadata.Listener);

                    Logger.Log($"Listening {metadata.GetPath()} {metadata.ToString()} channel ({metadata.Handlers.Count}): {string.Join(",", metadata.Names)}");
                }
            }

            Logger.Log("Listeners loaded");
        }
    }
}