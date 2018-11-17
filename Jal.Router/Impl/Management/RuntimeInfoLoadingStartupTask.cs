using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Management
{
    public class RuntimeInfoLoadingStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        private readonly ISagaExecutionCoordinator _sec;

        private readonly ILogger _logger;

        public RuntimeInfoLoadingStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaExecutionCoordinator sec)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
            _router = router;
            _logger = logger;
            _sec = sec;
        }

        public void Run()
        {
            PopulateRouteMetadataEndpoints(_sources);

            PopulateListenersMetadata();
        }

        private void PopulateListenersMetadata()
        {
            foreach (var item in _configuration.RuntimeInfo.RoutesMetadata)
            {
                foreach (var channel in item.Route.Channels)
                {
                    var listener = _configuration.RuntimeInfo.ListenersMetadata.FirstOrDefault(x => x.GetId() == channel.GetId());

                    if (listener != null)
                    {
                        listener.Handlers.Add(item.Handler);

                        listener.Names.Add(item.Name);
                    }
                    else
                    {
                        var newlistener = new ListenerMetadata(channel.ToPath, channel.ToConnectionString, channel.ToSubscription);

                        newlistener.Handlers.Add(item.Handler);

                        newlistener.Names.Add(item.Name);

                        _configuration.RuntimeInfo.ListenersMetadata.Add(newlistener);
                    }
                }
            }
        }

        private void PopulateRouteMetadataEndpoints(IRouterConfigurationSource[] sources)
        {
            foreach (var source in sources)
            {
                _configuration.RuntimeInfo.RoutesMetadata.AddRange(source.GetRoutes().Select(route => new RouteMetadata() { Route = route, Handler = message => _router.Route(message, route), Name = route.Name }));

                _configuration.RuntimeInfo.EndPoints.AddRange(source.GetEndPoints());

                _configuration.RuntimeInfo.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                _configuration.RuntimeInfo.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                _configuration.RuntimeInfo.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                foreach (var saga in source.GetSagas())
                {
                    if (saga.FirstRoute != null)
                    {
                        _configuration.RuntimeInfo.RoutesMetadata.Add(new RouteMetadata() { Route = saga.FirstRoute, Handler = message => _sec.Start(message, saga, saga.FirstRoute), Name = $"{saga.Name}/{saga.FirstRoute.Name}" });
                    }
                    if (saga.LastRoute != null)
                    {
                        _configuration.RuntimeInfo.RoutesMetadata.Add(new RouteMetadata() { Route = saga.LastRoute, Handler = message => _sec.End(message, saga, saga.LastRoute), Name = $"{saga.Name}/{saga.LastRoute.Name}" });
                    }

                    _configuration.RuntimeInfo.RoutesMetadata.AddRange(saga.Routes.Select(route => new RouteMetadata() { Route = route, Handler = message => _sec.Continue(message, saga, route), Name = $"{saga.Name}/{route.Name}" }));
                }
            }


        }
    }
}