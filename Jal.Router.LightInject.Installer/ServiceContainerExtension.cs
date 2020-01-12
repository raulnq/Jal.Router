using System.Reflection;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouter(this IServiceContainer container, IRouterConfigurationSource[] sources)
        {
            RegisterRouter(container);

            if (sources != null)
            {
                foreach (var source in sources)
                {
                    container.Register(typeof(IRouterConfigurationSource) , source.GetType(), source.GetType().FullName, new PerContainerLifetime());

                }
            }
        }


        public static void RegisterRouter(this IServiceContainer container, Assembly[] sourceassemblies)
        {
            RegisterRouter(container);

            if (sourceassemblies != null)
            {
                foreach (var assemblysource in sourceassemblies)
                {
                    foreach (var exportedType in assemblysource.ExportedTypes)
                    {
                        if (exportedType.IsSubclassOf(typeof(AbstractRouterConfigurationSource)))
                        {
                            container.Register(typeof(AbstractRouterConfigurationSource), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }

                        if (typeof(IValueFinder).IsAssignableFrom(exportedType))
                        {
                            container.Register(typeof(IValueFinder), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }

        public static void RegisterRouter(this IServiceContainer container)
        {
            container.Register<IShutdownTask, PointToPointChannelResourceDestructor>(typeof(PointToPointChannelResourceDestructor).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, PublishSubscribeChannelResourceDestructor>(typeof(PublishSubscribeChannelResourceDestructor).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, SubscriptionToPublishSubscribeChannelResourceDestructor>(typeof(SubscriptionToPublishSubscribeChannelResourceDestructor).FullName, new PerContainerLifetime());

            container.Register<IChannelShuffler, DefaultChannelShuffler>(typeof(DefaultChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<IChannelShuffler, FisherYatesChannelShuffler>(typeof(FisherYatesChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            container.Register<IHasher, Hasher>(new PerContainerLifetime());

            container.Register<IInMemoryTransport, InMemoryTransport>(new PerContainerLifetime());

            container.Register<IFileSystemTransport, FileSystemTransport>(new PerContainerLifetime());

            container.Register<IListenerContextLoader, ListenerContextLoader>(new PerContainerLifetime());

            container.Register<IRuntimeListenerContextLoader, RuntimeListenerContextLoader>(new PerContainerLifetime());

            container.Register<ISenderContextLoader, SenderContextLoader>(new PerContainerLifetime());

            container.Register<IRuntimeSenderContextLoader, RuntimeSenderContextLoader>(new PerContainerLifetime());

            container.Register<IParameterProvider, ParameterProvider>(new PerContainerLifetime());

            container.Register<IComponentFactoryGateway, ComponentFactoryGateway>(new PerContainerLifetime());

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

            container.Register<IStartupTask, SubscriptionToPublishSubscribeChannelResourceCreator>(typeof (SubscriptionToPublishSubscribeChannelResourceCreator).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, StartupBeatLogger>(typeof (StartupBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ShutdownTask>(typeof (ShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, EndpointsInitializer>(typeof (EndpointsInitializer).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, RoutesInitializer>(typeof(RoutesInitializer).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, RuntimeConfigurationLoader>(typeof(RuntimeConfigurationLoader).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, PointToPointChannelResourceCreator>(typeof(PointToPointChannelResourceCreator).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, PublishSubscribeChannelResourceCreator>(typeof(PublishSubscribeChannelResourceCreator).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ListenerLoader>(typeof (ListenerLoader).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, SenderLoader>(typeof(SenderLoader).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ListenerShutdownTask>(typeof (ListenerShutdownTask).FullName,new PerContainerLifetime());

            container.Register<IShutdownTask, SenderShutdownTask>(typeof(SenderShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, NullShutdownWatcher>(typeof (NullShutdownWatcher).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher, FileShutdownWatcher>(typeof (FileShutdownWatcher).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, CtrlCShutdownWatcher>(typeof(CtrlCShutdownWatcher).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, SignTermShutdownWatcher>(typeof(SignTermShutdownWatcher).FullName, new PerContainerLifetime());

            container.Register<IRouteErrorMessageHandler, ForwardRouteErrorMessageHandler>(typeof(ForwardRouteErrorMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IRouteErrorMessageHandler, RetryRouteErrorMessageHandler>(typeof(RetryRouteErrorMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IRouteEntryMessageHandler, ForwardRouteEntryMessageHandler>(typeof(ForwardRouteEntryMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IRouteEntryMessageHandler, RouteEntryMessageHandler>(typeof(RouteEntryMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<IEntityStorageGateway, EntityStorageGateway>(new PerContainerLifetime());

            container.Register<IMonitoringTask, PointToPointChannelMonitor>(typeof (PointToPointChannelMonitor).FullName,new PerContainerLifetime());

            container.Register<IMonitoringTask, SubscriptionToPublishSubscribeChannelMonitor>(typeof (SubscriptionToPublishSubscribeChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, HeartBeatLogger>(typeof (HeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, ListenerMonitor>(typeof(ListenerMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, ListenerRestartMonitor>(typeof(ListenerRestartMonitor).FullName, new PerContainerLifetime());

            container.Register<IMessageSerializer, NullMessageSerializer>(typeof (NullMessageSerializer).FullName,new PerContainerLifetime());

            container.Register<IMessageAdapter, NullMessageAdapter>(typeof (NullMessageAdapter).FullName,new PerContainerLifetime());

            container.Register<IMessageAdapter, MessageAdapter>(typeof(MessageAdapter).FullName, new PerContainerLifetime());

            container.Register<IMessageStorage, NullMessageStorage>(typeof(NullMessageStorage).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof (NullRouterInterceptor).FullName,new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof (NullPointToPointChannel).FullName);

            container.Register<IPointToPointChannel, FileSystemPointToPointChannel>(typeof(FileSystemPointToPointChannel).FullName);

            container.Register<IPublishSubscribeChannel, FileSystemPublishSubscribeChannel>(typeof(FileSystemPublishSubscribeChannel).FullName);

            container.Register<IPointToPointChannel, InMemoryPointToPointChannel>(typeof(InMemoryPointToPointChannel).FullName);

            container.Register<IPublishSubscribeChannel, InMemoryPublishSubscribeChannel>(typeof(InMemoryPublishSubscribeChannel).FullName);

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof (NullPublishSubscribeChannel).FullName);

            container.Register<IRequestReplyChannelFromPointToPointChannel, NullRequestReplyChannelFromPointToPointChannel>(typeof (NullRequestReplyChannelFromPointToPointChannel).FullName);

            container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(typeof(NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel).FullName);

            container.Register<IRequestReplyChannelFromPointToPointChannel, FileSystemRequestReplyFromPointToPointChannel>(typeof(FileSystemRequestReplyFromPointToPointChannel).FullName);

            container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>(typeof(FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName);

            container.Register<IRequestReplyChannelFromPointToPointChannel, InMemoryRequestReplyFromPointToPointChannel>(typeof(InMemoryRequestReplyFromPointToPointChannel).FullName);

            container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>(typeof(InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName);

            container.Register<IChannelResource, NullChannelResource>(typeof (NullChannelResource).FullName,new PerContainerLifetime());

            container.Register<IChannelResource, FileSystemChannelResource>(typeof(FileSystemChannelResource).FullName, new PerContainerLifetime());

            container.Register<IChannelResource, InMemoryChannelResource>(typeof(InMemoryChannelResource).FullName, new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof (NullBusInterceptor).FullName,new PerContainerLifetime());

            container.Register<ILogger<Beat>, BeatLogger>(typeof (BeatLogger).FullName,new PerContainerLifetime());

            container.Register<ILogger<PointToPointChannelStatistics>, PointToPointChannelStatisticsLogger>(typeof(PointToPointChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<PublishSubscribeChannelStatistics>, PublishSubscribeChannelStatisticsLogger>(typeof(PublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<SubscriptionToPublishSubscribeChannelStatistics>, SubscriptionToPublishSubscribeChannelStatisticsLogger>(typeof(SubscriptionToPublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<IEntityStorage, NullStorage>(typeof (NullStorage).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, Impl.ConsumerMiddleware>(typeof (Impl.ConsumerMiddleware).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, RouterMiddleware>(typeof (RouterMiddleware).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, InitialConsumerMiddleware>(typeof (InitialConsumerMiddleware).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, MiddleConsumerMiddleware>(typeof (MiddleConsumerMiddleware).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, FinalConsumerMiddleware>(typeof (FinalConsumerMiddleware).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, Impl.ProducerMiddleware>(typeof (Impl.ProducerMiddleware).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, BusMiddleware>(typeof(BusMiddleware).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, ConfigurationValueFinder>(typeof (ConfigurationValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, NullValueFinder>(typeof(NullValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, EnvironmentValueFinder>(typeof(EnvironmentValueFinder).FullName, new PerContainerLifetime());

            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof (EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());
        }
    }
}
