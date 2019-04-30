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

        public RuntimeConfigurationLoader(IComponentFactoryGateway factory, IRouterConfigurationSource[] sources, IRouter router, ILogger logger)
            :base(factory, logger)
        {
            _sources = sources;
            _router = router;
        }

        public Task Run()
        {
            Logger.Log("Loading runtime configuration");

            var adapter = Factory.CreateMessageAdapter();

            foreach (var source in _sources)
            {
                Factory.Configuration.Runtime.EndPoints.AddRange(source.GetEndPoints());

                Factory.Configuration.Runtime.Groups.AddRange(source.GetGroups());

                Factory.Configuration.Runtime.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                Factory.Configuration.Runtime.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                Factory.Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.RuntimeHandler = async (message, channel) => {

                        var context = await adapter.ReadMetadataAndContentFromRoute(message, route, channel).ConfigureAwait(false);

                        await _router.Route<MessageHandler>(context).ConfigureAwait(false);
                    };

                    Factory.Configuration.Runtime.Routes.Add(route);
                }
            }

            foreach (var saga in Factory.Configuration.Runtime.Sagas)
            {
                if (saga.InitialRoutes != null)
                {
                    foreach (var route in saga.InitialRoutes)
                    {
                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route, channel, saga).ConfigureAwait(false);

                            await _router.Route<InitialMessageHandler>(context).ConfigureAwait(false); ;
                        };

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if (saga.FinalRoutes != null)
                {
                    foreach (var route in saga.FinalRoutes)
                    {
                        Factory.Configuration.Runtime.Routes.Add(route);

                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route, channel, saga).ConfigureAwait(false);

                            await _router.Route<FinalMessageHandler>(context).ConfigureAwait(false); ;
                        };
                    }
                }

                if(saga.Routes!=null)
                {
                    foreach (var route in saga.Routes)
                    {
                        route.RuntimeHandler = async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route, channel, saga).ConfigureAwait(false);

                            await _router.Route<MiddleMessageHandler>(context).ConfigureAwait(false);
                        };

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }
            }

            Logger.Log("Runtime configuration loaded");

            return Task.CompletedTask;
        }
    }
}