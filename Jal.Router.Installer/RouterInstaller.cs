using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {
        private readonly Action<IRouterBuilder> _action;

        public RouterInstaller(Action<IRouterBuilder> action)
        {
            _action = action;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddChainOfResponsability();

            var builder = new RouterBuilder(container);

            builder.AddPointToPointChannel<NullPointToPointChannel>();

            builder.AddPublishSubscribeChannel<NullPublishSubscribeChannel>();

            builder.AddRequestReplyChannelFromPointToPointChannel<NullRequestReplyChannelFromPointToPointChannel>();

            builder.AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            builder.AddPointToPointChannel<FileSystemPointToPointChannel>();

            builder.AddPublishSubscribeChannel<FileSystemPublishSubscribeChannel>();

            builder.AddRequestReplyChannelFromPointToPointChannel<FileSystemRequestReplyFromPointToPointChannel>();

            builder.AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>();

            builder.AddPointToPointChannel<InMemoryPointToPointChannel>();

            builder.AddPublishSubscribeChannel<InMemoryPublishSubscribeChannel>();

            builder.AddRequestReplyChannelFromPointToPointChannel<InMemoryRequestReplyFromPointToPointChannel>();

            builder.AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>();

            builder.AddShutdownTask<ResourceDestructor>();

            builder.AddShutdownTask<ShutdownTask>();

            builder.AddShutdownTask<SenderShutdownTask>();

            builder.AddShutdownTask<ListenerShutdownTask>();

            builder.AddStartupTask<ResourceValidator>();

            builder.AddStartupTask<StartupBeatLogger>();

            builder.AddStartupTask<EndpointValidator>();

            builder.AddStartupTask<ResourceCreator>();

            builder.AddStartupTask<RouteValidator>();

            builder.AddStartupTask<RuntimeLoader>();

            builder.AddStartupTask<ListenerLoader>();

            builder.AddStartupTask<SenderLoader>();

            builder.AddMonitoringTask<StatisticMonitor>();

            builder.AddMonitoringTask<HeartBeatLogger>();

            builder.AddMonitoringTask<ListenerMonitor>();

            builder.AddMonitoringTask<ListenerRestartMonitor>();

            builder.AddMessageSerializer<NullMessageSerializer>();

            builder.AddMessageAdapter<NullMessageAdapter>();

            builder.AddMessageStorage<NullMessageStorage>();

            builder.AddEntityStorage<NullEntityStorage>();

            builder.AddEntityStorage<InMemoryEntityStorage>();

            builder.AddResourceManager<NullResourceManager>();

            builder.AddResourceManager<FileSystemPointToPointlResourceManager>();

            builder.AddResourceManager<FileSystemPublishSubscribeResourceManager>();

            builder.AddResourceManager<FileSystemSubscriptionToPublishSubscribeResourceManager>();

            builder.AddResourceManager<InMemoryPointToPointResourceManager>();

            builder.AddResourceManager<InMemoryPublishSubscribeResourceManager>();

            builder.AddResourceManager<InMemorySubscriptionToPublishSubscribeResourceManager>();

            builder.AddLogger<StatisticLogger, Statistic>();

            builder.AddLogger<BeatLogger, Beat>();

            builder.AddMiddleware<ConsumerMiddleware>();

            builder.AddMiddleware<RouterMiddleware>();

            builder.AddMiddleware<InitialConsumerMiddleware>();

            builder.AddMiddleware<MiddleConsumerMiddleware>();

            builder.AddMiddleware<FinalConsumerMiddleware>();

            builder.AddMiddleware<BusMiddleware>();

            builder.AddMiddleware<ProducerMiddleware>();

            builder.AddSource<EmptyRouterConfigurationSource>();

            builder.AddShutdownWatcher<NullShutdownWatcher>();

            builder.AddShutdownWatcher<FileShutdownWatcher>();

            builder.AddShutdownWatcher<CtrlCShutdownWatcher>();

            builder.AddShutdownWatcher<SignTermShutdownWatcher>();

            builder.AddChannelShuffler<DefaultChannelShuffler>();

            builder.AddChannelShuffler<FisherYatesChannelShuffler>();

            builder.AddRouterInterceptor<NullRouterInterceptor>();

            builder.AddBusInterceptor<NullBusInterceptor>();

            builder.AddMessageAdapter<MessageAdapter>();

            builder.AddRouteErrorMessageHandler<ForwardRouteErrorMessageHandler>();

            builder.AddRouteErrorMessageHandler<RetryRouteErrorMessageHandler>();

            builder.AddRouteEntryMessageHandler<ForwardRouteEntryMessageHandler>();

            builder.AddRouteEntryMessageHandler<RouteEntryMessageHandler>();

            container.Register(Component.For<IComponentFactoryFacade>().ImplementedBy<ComponentFactoryFacade>().LifestyleSingleton());

            container.Register(Component.For<IHost>().ImplementedBy<Host>().Named(typeof(Host).FullName).LifestyleSingleton());

            container.Register(Component.For<IEntityStorageFacade>().ImplementedBy<EntityStorageFacade>().LifestyleSingleton());

            container.Register(Component.For<IInMemoryTransport>().ImplementedBy<InMemoryTransport>().LifestyleSingleton());

            container.Register(Component.For<IFileSystemTransport>().ImplementedBy<FileSystemTransport>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextLifecycle>().ImplementedBy<ListenerContextLifecycle>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextLoader>().ImplementedBy<ListenerContextLoader>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextLifecycle>().ImplementedBy<SenderContextLifecycle>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextLoader>().ImplementedBy<SenderContextLoader>().LifestyleSingleton());

            container.Register(Component.For<ILogger>().ImplementedBy<Impl.ConsoleLogger>().LifestyleSingleton());

            container.Register(Component.For<IRouter>().ImplementedBy<Impl.Router>().LifestyleSingleton());

            container.Register(Component.For<IParameterProvider>().ImplementedBy<ParameterProvider>().LifestyleSingleton());

            container.Register(Component.For<IConsumer>().ImplementedBy<Consumer>().LifestyleSingleton());

            container.Register(Component.For<ITypedConsumer>().ImplementedBy<TypedConsumer>().LifestyleSingleton());

            container.Register(Component.For<IEndPointProvider>().ImplementedBy<EndPointProvider>().LifestyleSingleton());

            container.Register(Component.For<IBus>().ImplementedBy<Bus>().LifestyleSingleton());

            container.Register(Component.For<IProducer>().ImplementedBy<Producer>().LifestyleSingleton());

            container.Register(Component.For<IComponentFactory>().ImplementedBy<ComponentFactory>().LifestyleSingleton());

            container.Register(Component.For<IStartup>().ImplementedBy<Startup>().LifestyleSingleton());

            container.Register(Component.For<IShutdown>().ImplementedBy<Shutdown>().LifestyleSingleton());

            container.Register(Component.For<IConfiguration>().ImplementedBy<Configuration>().LifestyleSingleton());

            container.Register(Component.For<IChannelValidator>().ImplementedBy<ChannelValidator>().LifestyleSingleton());

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            if (_action!=null)
            {
                _action(builder);
            }
        }
    }
}
