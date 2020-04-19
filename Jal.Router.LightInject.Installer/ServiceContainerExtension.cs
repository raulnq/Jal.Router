using Jal.ChainOfResponsability.LightInject.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;
using System;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddRouter(this IServiceContainer container, Action<IRouterBuilder> action = null)
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

            builder.AddShutdownTask<PointToPointChannelResourceDestructor>();

            builder.AddShutdownTask<PublishSubscribeChannelResourceDestructor>();

            builder.AddShutdownTask<SubscriptionToPublishSubscribeChannelResourceDestructor>();

            builder.AddShutdownTask<ShutdownTask>();

            builder.AddShutdownTask<SenderShutdownTask>();

            builder.AddShutdownTask<ListenerShutdownTask>();

            builder.AddStartupTask<PointToPointChannelResourceValidator>();

            builder.AddStartupTask<PublishSubscribeChannelResourceValidator>();

            builder.AddStartupTask<SubscriptionToPublishSubscribeChannelResourceValidator>();

            builder.AddStartupTask<SubscriptionToPublishSubscribeChannelResourceCreator>();

            builder.AddStartupTask<StartupBeatLogger>();

            builder.AddStartupTask<EndpointValidator>();

            builder.AddStartupTask<PointToPointChannelResourceCreator>();

            builder.AddStartupTask<PublishSubscribeChannelResourceCreator>();

            builder.AddStartupTask<RouteValidator>();

            builder.AddStartupTask<RuntimeLoader>();

            builder.AddStartupTask<ListenerLoader>();

            builder.AddStartupTask<SenderLoader>();

            builder.AddMonitoringTask<PointToPointChannelMonitor>();

            builder.AddMonitoringTask<SubscriptionToPublishSubscribeChannelMonitor>();

            builder.AddMonitoringTask<HeartBeatLogger>();

            builder.AddMonitoringTask<ListenerMonitor>();

            builder.AddMonitoringTask<ListenerRestartMonitor>();

            builder.AddMessageSerializer<NullMessageSerializer>();

            builder.AddMessageAdapter<NullMessageAdapter>();

            builder.AddMessageStorage<NullMessageStorage>();

            builder.AddEntityStorage<NullEntityStorage>();

            builder.AddEntityStorage<InMemoryEntityStorage>();

            builder.AddChannelResourceManager<NullChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>, PointToPointChannelResource, PointToPointChannelStatistics>();

            builder.AddChannelResourceManager<NullChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>, PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<NullChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>, SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<FileSystemPointToPointChannelResourceManager, PointToPointChannelResource, PointToPointChannelStatistics>();

            builder.AddChannelResourceManager<FileSystemPublishSubscribeChannelResourceManager, PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<FileSystemSubscriptionToPublishSubscribeChannelResourceManager, SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<InMemoryPointToPointChannelResourceManager, PointToPointChannelResource, PointToPointChannelStatistics>();

            builder.AddChannelResourceManager<InMemoryPublishSubscribeChannelResourceManager, PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>();

            builder.AddChannelResourceManager<InMemorySubscriptionToPublishSubscribeChannelResourceManager, SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddLogger<PointToPointChannelStatisticsLogger, PointToPointChannelStatistics>();

            builder.AddLogger<PublishSubscribeChannelStatisticsLogger, PublishSubscribeChannelStatistics>();

            builder.AddLogger<SubscriptionToPublishSubscribeChannelStatisticsLogger, SubscriptionToPublishSubscribeChannelStatistics>();

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

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<IEntityStorageFacade, EntityStorageFacade>(new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            container.Register<IHasher, Hasher>(new PerContainerLifetime());

            container.Register<IInMemoryTransport, InMemoryTransport>(new PerContainerLifetime());

            container.Register<IFileSystemTransport, FileSystemTransport>(new PerContainerLifetime());

            container.Register<IListenerContextLifecycle, ListenerContextLifecycle>(new PerContainerLifetime());

            container.Register<IListenerContextLoader, ListenerContextLoader>(new PerContainerLifetime());

            container.Register<ISenderContextLifecycle, SenderContextLifecycle>(new PerContainerLifetime());

            container.Register<ISenderContextLoader, SenderContextLoader>(new PerContainerLifetime());

            container.Register<IParameterProvider, ParameterProvider>(new PerContainerLifetime());

            container.Register<IComponentFactoryFacade, ComponentFactoryFacade>(new PerContainerLifetime());

            container.Register<IHost, Host>(new PerContainerLifetime());

            container.Register<IProducer, Producer>(new PerContainerLifetime());

            container.Register<IRouter, Impl.Router>(new PerContainerLifetime());

            container.Register<IConsumer, Consumer>(new PerContainerLifetime());

            container.Register<ITypedConsumer, TypedConsumer>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IShutdown, Shutdown>(new PerContainerLifetime());

            container.Register<IChannelValidator, ChannelValidator>(new PerContainerLifetime());

            if (action!=null)
            {
                action(builder);
            }
        }
    }
}
