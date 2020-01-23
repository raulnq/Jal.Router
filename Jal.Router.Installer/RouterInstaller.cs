using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _sourceassemblies;

        private readonly IRouterConfigurationSource[] _sources;

        public RouterInstaller(Assembly[] sourceassemblies)
        {
            _sourceassemblies = sourceassemblies;
        }

        public RouterInstaller(IRouterConfigurationSource[] sources)
        {
            _sources = sources;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRequestReplyChannelFromPointToPointChannel>().ImplementedBy<FileSystemRequestReplyFromPointToPointChannel>().Named(typeof(FileSystemRequestReplyFromPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>().ImplementedBy<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>().Named(typeof(FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IRequestReplyChannelFromPointToPointChannel>().ImplementedBy<InMemoryRequestReplyFromPointToPointChannel>().Named(typeof(InMemoryRequestReplyFromPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>().ImplementedBy<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>().Named(typeof(InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<FileSystemPointToPointChannel>().Named(typeof(FileSystemPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<FileSystemPublishSubscribeChannel>().Named(typeof(FileSystemPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<InMemoryPointToPointChannel>().Named(typeof(InMemoryPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<InMemoryPublishSubscribeChannel>().Named(typeof(InMemoryPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IInMemoryTransport>().ImplementedBy<InMemoryTransport>().LifestyleSingleton());

            container.Register(Component.For<IFileSystemTransport>().ImplementedBy<FileSystemTransport>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextCreator>().ImplementedBy<ListenerContextCreator>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextLoader>().ImplementedBy<ListenerContextLoader>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextCreator>().ImplementedBy<SenderContextCreator>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextLoader>().ImplementedBy<SenderContextLoader>().LifestyleSingleton());

            container.Register(Component.For<IShutdownTask>().ImplementedBy<PointToPointChannelResourceDestructor>().LifestyleSingleton().Named(typeof(PointToPointChannelResourceDestructor).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<PublishSubscribeChannelResourceDestructor>().LifestyleSingleton().Named(typeof(PublishSubscribeChannelResourceDestructor).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelResourceDestructor>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelResourceDestructor).FullName));

            container.Register(Component.For<IChannelShuffler>().ImplementedBy<DefaultChannelShuffler>().LifestyleSingleton().Named(typeof(DefaultChannelShuffler).FullName));

            container.Register(Component.For<IChannelShuffler>().ImplementedBy<FisherYatesChannelShuffler>().LifestyleSingleton().Named(typeof(FisherYatesChannelShuffler).FullName));

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

            container.Register(Component.For<IStartupTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelResourceCreator>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelResourceCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<Impl.StartupBeatLogger>().LifestyleSingleton().Named(typeof(Impl.StartupBeatLogger).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<ShutdownTask>().LifestyleSingleton().Named(typeof(ShutdownTask).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<SenderShutdownTask>().LifestyleSingleton().Named(typeof(SenderShutdownTask).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<EndpointsInitializer>().LifestyleSingleton().Named(typeof(EndpointsInitializer).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<PointToPointChannelResourceCreator>().LifestyleSingleton().Named(typeof(PointToPointChannelResourceCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<PublishSubscribeChannelResourceCreator>().LifestyleSingleton().Named(typeof(PublishSubscribeChannelResourceCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<RoutesInitializer>().LifestyleSingleton().Named(typeof(RoutesInitializer).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<RuntimeConfigurationLoader>().LifestyleSingleton().Named(typeof(RuntimeConfigurationLoader).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<NullShutdownWatcher>().LifestyleSingleton().Named(typeof(NullShutdownWatcher).FullName));

            container.Register(Component.For(typeof(IStartupTask)).ImplementedBy(typeof(ListenerLoader)).Named(typeof(ListenerLoader).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IStartupTask)).ImplementedBy(typeof(SenderLoader)).Named(typeof(SenderLoader).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IShutdownTask)).ImplementedBy(typeof(ListenerShutdownTask)).Named(typeof(ListenerShutdownTask).FullName).LifestyleSingleton());

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            container.Register(Component.For<IEntityStorageGateway>().ImplementedBy<EntityStorageGateway>().LifestyleSingleton());

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<PointToPointChannelMonitor>().LifestyleSingleton().Named(typeof(PointToPointChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelMonitor>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<HeartBeatLogger>().LifestyleSingleton().Named(typeof(HeartBeatLogger).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<ListenerMonitor>().LifestyleSingleton().Named(typeof(ListenerMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<ListenerRestartMonitor>().LifestyleSingleton().Named(typeof(ListenerRestartMonitor).FullName));

            container.Register(Component.For<IMessageSerializer>().ImplementedBy<NullMessageSerializer>().LifestyleSingleton().Named(typeof(NullMessageSerializer).FullName));

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<NullMessageAdapter>().LifestyleSingleton().Named(typeof(NullMessageAdapter).FullName));

            container.Register(Component.For<IMessageStorage>().ImplementedBy<NullMessageStorage>().LifestyleSingleton().Named(typeof(NullMessageStorage).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<FileShutdownWatcher>().LifestyleSingleton().Named(typeof(FileShutdownWatcher).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<CtrlCShutdownWatcher>().LifestyleSingleton().Named(typeof(CtrlCShutdownWatcher).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<SignTermShutdownWatcher>().LifestyleSingleton().Named(typeof(SignTermShutdownWatcher).FullName));

            container.Register(Component.For<IRouteErrorMessageHandler>().ImplementedBy<ForwardRouteErrorMessageHandler>().LifestyleSingleton().Named(typeof(ForwardRouteErrorMessageHandler).FullName));

            container.Register(Component.For<IRouteErrorMessageHandler>().ImplementedBy<RetryRouteErrorMessageHandler>().LifestyleSingleton().Named(typeof(RetryRouteErrorMessageHandler).FullName));

            container.Register(Component.For<IRouteEntryMessageHandler>().ImplementedBy<ForwardRouteEntryMessageHandler>().LifestyleSingleton().Named(typeof(ForwardRouteEntryMessageHandler).FullName));

            container.Register(Component.For<IRouteEntryMessageHandler>().ImplementedBy<RouteEntryMessageHandler>().LifestyleSingleton().Named(typeof(RouteEntryMessageHandler).FullName));

            container.Register(Component.For(typeof(IRouterInterceptor)).ImplementedBy(typeof(NullRouterInterceptor)).Named(typeof(NullRouterInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPointToPointChannel)).ImplementedBy(typeof(NullPointToPointChannel)).Named(typeof(NullPointToPointChannel).FullName).LifestyleTransient());

            container.Register(Component.For(typeof(IPublishSubscribeChannel)).ImplementedBy(typeof(NullPublishSubscribeChannel)).Named(typeof(NullPublishSubscribeChannel).FullName).LifestyleTransient());

            container.Register(Component.For(typeof(IRequestReplyChannelFromPointToPointChannel)).ImplementedBy(typeof(NullRequestReplyChannelFromPointToPointChannel)).Named(typeof(NullRequestReplyChannelFromPointToPointChannel).FullName).LifestyleTransient());

            container.Register(Component.For(typeof(IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel)).ImplementedBy(typeof(NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel)).Named(typeof(NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel).FullName).LifestyleTransient());

            container.Register(Component.For<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<NullChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().LifestyleSingleton().Named(typeof(NullChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>).FullName));

            container.Register(Component.For<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<NullChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().LifestyleSingleton().Named(typeof(NullChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>).FullName));

            container.Register(Component.For<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<NullChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().LifestyleSingleton().Named(typeof(NullChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>).FullName));

            container.Register(Component.For<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<FileSystemPointToPointChannelResource>().LifestyleSingleton().Named(typeof(FileSystemPointToPointChannelResource).FullName));

            container.Register(Component.For<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<FileSystemPublishSubscribeChannelResource>().LifestyleSingleton().Named(typeof(FileSystemPublishSubscribeChannelResource).FullName));

            container.Register(Component.For<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<FileSystemSubscriptionToPublishSubscribeChannelResource>().LifestyleSingleton().Named(typeof(FileSystemSubscriptionToPublishSubscribeChannelResource).FullName));

            container.Register(Component.For<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<InMemoryPointToPointChannelResource>().LifestyleSingleton().Named(typeof(InMemoryPointToPointChannelResource).FullName));

            container.Register(Component.For<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<InMemoryPublishSubscribeChannelResource>().LifestyleSingleton().Named(typeof(InMemoryPublishSubscribeChannelResource).FullName));

            container.Register(Component.For<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<InMemorySubscriptionToPublishSubscribeChannelResource>().LifestyleSingleton().Named(typeof(InMemorySubscriptionToPublishSubscribeChannelResource).FullName));

            container.Register(Component.For(typeof(ILogger<PointToPointChannelStatistics>)).ImplementedBy(typeof(Impl.PointToPointChannelStatisticsLogger)).Named(typeof(Impl.PointToPointChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<PublishSubscribeChannelStatistics>)).ImplementedBy(typeof(Impl.PublishSubscribeChannelStatisticsLogger)).Named(typeof(Impl.PublishSubscribeChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<SubscriptionToPublishSubscribeChannelStatistics>)).ImplementedBy(typeof(Impl.SubscriptionToPublishSubscribeChannelStatisticsLogger)).Named(typeof(Impl.SubscriptionToPublishSubscribeChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<Beat>)).ImplementedBy(typeof(Impl.BeatLogger)).Named(typeof(Impl.BeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IBusInterceptor)).ImplementedBy(typeof(NullBusInterceptor)).Named(typeof(NullBusInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IEntityStorage)).ImplementedBy(typeof(NullStorage)).Named(typeof(NullStorage).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddlewareAsync<MessageContext>)).ImplementedBy(typeof(Impl.ConsumerMiddleware)).Named(typeof(Impl.ConsumerMiddleware).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddlewareAsync<MessageContext>)).ImplementedBy(typeof(RouterMiddleware)).Named(typeof(RouterMiddleware).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddlewareAsync<MessageContext>)).ImplementedBy(typeof(InitialConsumerMiddleware)).Named(typeof(InitialConsumerMiddleware).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddlewareAsync<MessageContext>)).ImplementedBy(typeof(MiddleConsumerMiddleware)).Named(typeof(MiddleConsumerMiddleware).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddlewareAsync<MessageContext>)).ImplementedBy(typeof(FinalConsumerMiddleware)).Named(typeof(FinalConsumerMiddleware).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddlewareAsync<MessageContext>>().ImplementedBy<BusMiddleware>().LifestyleSingleton().Named(typeof(BusMiddleware).FullName));

            container.Register(Component.For<IMiddlewareAsync<MessageContext>>().ImplementedBy<Impl.ProducerMiddleware>().LifestyleSingleton().Named(typeof(Impl.ProducerMiddleware).FullName));

            container.Register(Component.For<IComponentFactoryGateway>().ImplementedBy<ComponentFactoryGateway>().LifestyleSingleton().Named(typeof(ComponentFactoryGateway).FullName));

            container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(typeof(EmptyRouterConfigurationSource)).Named(typeof(EmptyRouterConfigurationSource).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IHost)).ImplementedBy(typeof(Host)).Named(typeof(Host).FullName).LifestyleSingleton());

            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                }
            }

            if (_sources != null)
            {
                foreach (var source in _sources)
                {
                    container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(source.GetType()).Named(source.GetType().FullName).LifestyleSingleton());

                }
            }
        }
    }
}
