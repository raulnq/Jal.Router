using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class RuntimeLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        public RuntimeLoader(IComponentFactoryGateway factory, IRouterConfigurationSource[] sources, IRouter router, ILogger logger)
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

                Factory.Configuration.Runtime.PointToPointChannelResources.AddRange(source.GetPointToPointChannelResources());

                Factory.Configuration.Runtime.PublishSubscribeChannelResources.AddRange(source.GetPublishSubscribeChannelResources());

                Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.AddRange(source.GetSubscriptionsToPublishSubscribeChannelResource());

                Factory.Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    route.SetConsumer(async (message, channel) => {

                        var context = await adapter.ReadMetadataAndContentFromPhysicalMessage(message, route.ContentType, route.UseClaimCheck).ConfigureAwait(false);

                        context.SetRoute(route);

                        context.SetChannel(channel);

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
                        route.SetConsumer(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromPhysicalMessage(message, route.ContentType, route.UseClaimCheck).ConfigureAwait(false);

                            context.SetRoute(route);

                            context.SetChannel(channel);

                            context.SetSaga(saga);

                            await _router.Route<InitialConsumerMiddleware>(context).ConfigureAwait(false); ;
                        });

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if (saga.FinalRoutes != null)
                {
                    foreach (var route in saga.FinalRoutes)
                    {
                        route.SetConsumer(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromPhysicalMessage(message, route.ContentType, route.UseClaimCheck).ConfigureAwait(false);

                            context.SetRoute(route);

                            context.SetChannel(channel);

                            context.SetSaga(saga);

                            await _router.Route<FinalConsumerMiddleware>(context).ConfigureAwait(false); ;
                        });

                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if(saga.Routes!=null)
                {
                    foreach (var route in saga.Routes)
                    {
                        route.SetConsumer(async (message, channel) => {

                            var context = await adapter.ReadMetadataAndContentFromPhysicalMessage(message, route.ContentType, route.UseClaimCheck).ConfigureAwait(false);

                            context.SetRoute(route);

                            context.SetChannel(channel);

                            context.SetSaga(saga);

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