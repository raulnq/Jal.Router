using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class RuntimeConfigurationLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        public RuntimeConfigurationLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger)
            :base(factory, configuration, logger)
        {
            _sources = sources;
            _router = router;
        }

        public void Run()
        {
            Logger.Log("Loading runtime configuration");

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            foreach (var source in _sources)
            {
                Configuration.Runtime.EndPoints.AddRange(source.GetEndPoints());

                Configuration.Runtime.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                Configuration.Runtime.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                Configuration.Runtime.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.RuntimeHandler = (message, channel) => {

                        var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

                        context.Channel = channel;

                        context.Route = route;

                        _router.Route<MessageHandler>(context);
                    };

                    Configuration.Runtime.Routes.Add(route);
                }
            }

            foreach (var saga in Configuration.Runtime.Sagas)
            {
                if (saga.FirstRoute != null)
                {
                    Configuration.Runtime.Routes.Add(saga.FirstRoute);

                    saga.FirstRoute.RuntimeHandler = (message, channel) => {

                        var context = adapter.Read(message, saga.FirstRoute.ContentType, saga.FirstRoute.UseClaimCheck, saga.FirstRoute.IdentityConfiguration);

                        context.Channel = channel;

                        context.Route = saga.FirstRoute;

                        context.Saga = saga;

                        _router.Route<FirstMessageHandler>(context);
                    };
                }

                if (saga.LastRoute != null)
                {
                    Configuration.Runtime.Routes.Add(saga.LastRoute);

                    saga.LastRoute.RuntimeHandler = (message, channel) => {

                        var context = adapter.Read(message, saga.LastRoute.ContentType, saga.LastRoute.UseClaimCheck, saga.LastRoute.IdentityConfiguration);

                        context.Channel = channel;

                        context.Route = saga.LastRoute;

                        context.Saga = saga;

                        _router.Route<LastMessageHandler>(context);
                    };
                }

                foreach (var route in saga.Routes)
                {
                    route.RuntimeHandler = (message, channel) => {

                        var context = adapter.Read(message, route.ContentType, route.UseClaimCheck, route.IdentityConfiguration);

                        context.Channel = channel;

                        context.Route = route;

                        context.Saga = saga;

                        _router.Route<MiddleMessageHandler>(context);
                    };

                    Configuration.Runtime.Routes.Add(route);
                }
            }

            Logger.Log("Runtime configuration loaded");
        }
    }
}