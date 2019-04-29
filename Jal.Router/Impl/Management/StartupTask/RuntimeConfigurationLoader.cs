using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using System.Threading.Tasks;

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

        public Task Run()
        {
            Logger.Log("Loading runtime configuration");

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            foreach (var source in _sources)
            {
                Configuration.Runtime.EndPoints.AddRange(source.GetEndPoints());

                Configuration.Runtime.Groups.AddRange(source.GetGroups());

                Configuration.Runtime.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                Configuration.Runtime.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                Configuration.Runtime.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.RuntimeHandler = async (message, channel) => {

                        var context = await adapter.ReadMetadataAndContentFromRoute(message, route);

                        context.Channel = channel;

                        context.Route = route;

                        await _router.Route<MessageHandler>(context);
                    };

                    Configuration.Runtime.Routes.Add(route);
                }
            }

            foreach (var saga in Configuration.Runtime.Sagas)
            {
                if (saga.InitialRoutes != null)
                {
                    foreach (var route in saga.InitialRoutes)
                    {
                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route);

                            context.Channel = channel;

                            context.Route = route;

                            context.Saga = saga;

                            await _router.Route<InitialMessageHandler>(context);
                        };

                        Configuration.Runtime.Routes.Add(route);
                    }
                }

                if (saga.FinalRoutes != null)
                {
                    foreach (var route in saga.FinalRoutes)
                    {
                        Configuration.Runtime.Routes.Add(route);

                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route);

                            context.Channel = channel;

                            context.Route = route;

                            context.Saga = saga;

                            await _router.Route<FinalMessageHandler>(context);
                        };
                    }
                }

                if(saga.Routes!=null)
                {
                    foreach (var route in saga.Routes)
                    {
                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route);

                            context.Channel = channel;

                            context.Route = route;

                            context.Saga = saga;

                            await _router.Route<MiddleMessageHandler>(context);
                        };

                        Configuration.Runtime.Routes.Add(route);
                    }
                }
            }

            Logger.Log("Runtime configuration loaded");

            return Task.CompletedTask;
        }
    }
}