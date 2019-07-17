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

                Factory.Configuration.Runtime.Partitions.AddRange(source.GetPartitions());

                Factory.Configuration.Runtime.PointToPointChannels.AddRange(source.GetPointToPointChannels());

                Factory.Configuration.Runtime.PublishSubscribeChannels.AddRange(source.GetPublishSubscribeChannels());

                Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannels.AddRange(source.GetSubscriptionsToPublishSubscribeChannel());

                Factory.Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.UpdateRuntimeHandler(async (message, channel) => {

                        var context = await adapter.ReadMetadataAndContentFromRoute(message, route).ConfigureAwait(false);

                        context.UpdateChannel(channel);

                        await _router.Route<ConsumerMiddleware>(context).ConfigureAwait(false);
                    });

                    Factory.Configuration.Runtime.Routes.Add(route);
                }
            }

            foreach (var saga in Factory.Configuration.Runtime.Sagas)
            {
                if (saga.InitialRoutes != null)
                {
                    foreach (var route in saga.InitialRoutes)
                    {
                        route.UpdateRuntimeHandler(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route).ConfigureAwait(false);

                            context.UpdateChannel(channel);

                            context.UpdateSaga(saga);

                            await _router.Route<InitialConsumerMiddleware>(context).ConfigureAwait(false); ;
                        });

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if (saga.FinalRoutes != null)
                {
                    foreach (var route in saga.FinalRoutes)
                    {
                        Factory.Configuration.Runtime.Routes.Add(route);

                        route.UpdateRuntimeHandler(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route).ConfigureAwait(false);

                            context.UpdateChannel(channel);

                            context.UpdateSaga(saga);

                            await _router.Route<FinalConsumerMiddleware>(context).ConfigureAwait(false); ;
                        });
                    }
                }

                if(saga.Routes!=null)
                {
                    foreach (var route in saga.Routes)
                    {
                        route.UpdateRuntimeHandler(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromRoute(message, route).ConfigureAwait(false);

                            context.UpdateChannel(channel);

                            context.UpdateSaga(saga);

                            await _router.Route<MiddleConsumerMiddleware>(context).ConfigureAwait(false);
                        });

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }
            }

            Logger.Log("Runtime configuration loaded");

            return Task.CompletedTask;
        }
    }
}