using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.StartupTask
{
    public class RuntimeConfigurationLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        private readonly ISagaExecutionCoordinator _sec;

        public RuntimeConfigurationLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaExecutionCoordinator sec)
            :base(factory, configuration, logger)
        {
            _sources = sources;
            _router = router;
            _sec = sec;
        }

        public void Run()
        {
            Logger.Log("Loading runtime configuration");

            foreach (var source in _sources)
            {
                Configuration.Runtime.EndPoints.AddRange(source.GetEndPoints());

                Configuration.Runtime.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                Configuration.Runtime.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                Configuration.Runtime.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.RuntimeHandler = message => _router.Route(message, route);

                    Configuration.Runtime.Routes.Add(route);
                }

                foreach (var saga in Configuration.Runtime.Sagas)
                {
                    if (saga.FirstRoute != null)
                    {
                        Configuration.Runtime.Routes.Add(saga.FirstRoute);

                        saga.FirstRoute.RuntimeHandler = message => _sec.Start(message, saga, saga.FirstRoute);
                    }

                    if (saga.LastRoute != null)
                    {
                        Configuration.Runtime.Routes.Add(saga.LastRoute);

                        saga.LastRoute.RuntimeHandler = message => _sec.End(message, saga, saga.LastRoute);
                    }

                    foreach (var route in saga.Routes)
                    {
                        route.RuntimeHandler = message => _sec.Continue(message, saga, route);

                        Configuration.Runtime.Routes.Add(route);
                    }
                }
            }

            Logger.Log("Runtime configuration loaded");
        }
    }
}