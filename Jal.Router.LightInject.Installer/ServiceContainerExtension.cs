using System.Reflection;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Impl.Management;
using Jal.Router.Impl.Management.ShutdownWatcher;
using Jal.Router.Impl.MonitoringTask;
using Jal.Router.Impl.Outbound;
using Jal.Router.Impl.Outbound.ChannelShuffler;
using Jal.Router.Impl.Outbound.Middleware;
using Jal.Router.Impl.ShutdownTask;
using Jal.Router.Impl.StartupTask;
using Jal.Router.Impl.ValueFinder;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;
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
            container.Register<IShutdownTask, PointToPointChannelDestructor>(typeof(PointToPointChannelDestructor).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, PublishSubscribeChannelDestructor>(typeof(PublishSubscribeChannelDestructor).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, SubscriptionToPublishSubscribeChannelDestructor>(typeof(SubscriptionToPublishSubscribeChannelDestructor).FullName, new PerContainerLifetime());

            container.Register<IChannelShuffler, DefaultChannelShuffler>(typeof(DefaultChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<IChannelShuffler, FisherYatesChannelShuffler>(typeof(FisherYatesChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            container.Register<IParameterProvider, ParameterProvider>(new PerContainerLifetime());

            container.Register<IComponentFactoryGateway, ComponentFactoryGateway>(new PerContainerLifetime());

            container.Register<IHost, Host>(new PerContainerLifetime());

            container.Register<ISender, Sender>(new PerContainerLifetime());

            container.Register<IRouter, Impl.Inbound.Router>(new PerContainerLifetime());

            container.Register<IMessageRouter, MessageRouter>(new PerContainerLifetime());

            container.Register<IHandlerMethodSelector, HandlerMethodSelector>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IHandlerMethodExecutor, HandlerMethodExecutor>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IShutdown, Shutdown>(new PerContainerLifetime());

            container.Register<IStartupTask, SubscriptionToPublishSubscribeChannelCreator>(typeof (SubscriptionToPublishSubscribeChannelCreator).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, StartupBeatLogger>(typeof (StartupBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ShutdownTask>(typeof (ShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, EndpointsInitializer>(typeof (EndpointsInitializer).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, RoutesInitializer>(typeof(RoutesInitializer).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, RuntimeConfigurationLoader>(typeof(RuntimeConfigurationLoader).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, PointToPointChannelCreator>(typeof(PointToPointChannelCreator).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, PublishSubscribeChannelCreator>(typeof(PublishSubscribeChannelCreator).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ListenerLoader>(typeof (ListenerLoader).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, SenderLoader>(typeof(SenderLoader).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ListenerShutdownTask>(typeof (ListenerShutdownTask).FullName,new PerContainerLifetime());

            container.Register<IShutdownTask, SenderShutdownTask>(typeof(SenderShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, NullShutdownWatcher>(typeof (NullShutdownWatcher).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher, FileShutdownWatcher>(typeof (FileShutdownWatcher).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, CtrlCShutdownWatcher>(typeof(CtrlCShutdownWatcher).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, SignTermShutdownWatcher>(typeof(SignTermShutdownWatcher).FullName, new PerContainerLifetime());
            
            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<IEntityStorageGateway, EntityStorageGateway>(new PerContainerLifetime());

            container.Register<IMonitoringTask, PointToPointChannelMonitor>(typeof (PointToPointChannelMonitor).FullName,new PerContainerLifetime());

            container.Register<IMonitoringTask, SubscriptionToPublishSubscribeChannelMonitor>(typeof (SubscriptionToPublishSubscribeChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, HeartBeatLogger>(typeof (HeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, ListenerMonitor>(typeof(ListenerMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, ListenerRestartMonitor>(typeof(ListenerRestartMonitor).FullName, new PerContainerLifetime());

            container.Register<IMessageSerializer, NullMessageSerializer>(typeof (NullMessageSerializer).FullName,new PerContainerLifetime());

            container.Register<IMessageAdapter, NullMessageAdapter>(typeof (NullMessageAdapter).FullName,new PerContainerLifetime());

            container.Register<IMessageStorage, NullMessageStorage>(typeof(NullMessageStorage).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof (NullRouterInterceptor).FullName,new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof (NullPointToPointChannel).FullName);

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof (NullPublishSubscribeChannel).FullName);

            container.Register<IRequestReplyChannelFromPointToPointChannel, NullRequestReplyChannelFromPointToPointChannel>(typeof (NullRequestReplyChannelFromPointToPointChannel).FullName);

            container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(typeof(NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel).FullName);

            container.Register<IChannelManager, NullChannelManager>(typeof (NullChannelManager).FullName,new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof (NullBusInterceptor).FullName,new PerContainerLifetime());

            container.Register<ILogger<Beat>, BeatLogger>(typeof (BeatLogger).FullName,new PerContainerLifetime());

            container.Register<ILogger<PointToPointChannelStatistics>, PointToPointChannelStatisticsLogger>(typeof(PointToPointChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<PublishSubscribeChannelStatistics>, PublishSubscribeChannelStatisticsLogger>(typeof(PublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<SubscriptionToPublishSubscribeChannelStatistics>, SubscriptionToPublishSubscribeChannelStatisticsLogger>(typeof(SubscriptionToPublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            container.Register<IEntityStorage, NullStorage>(typeof (NullStorage).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, Impl.Inbound.Middleware.MessageHandler>(typeof (Impl.Inbound.Middleware.MessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, MessageExceptionHandler>(typeof (MessageExceptionHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, InitialMessageHandler>(typeof (InitialMessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, MiddleMessageHandler>(typeof (MiddleMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, FinalMessageHandler>(typeof (FinalMessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, Impl.Outbound.Middleware.MessageHandler>(typeof (Impl.Outbound.Middleware.MessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddlewareAsync<MessageContext>, DistributionHandler>(typeof(DistributionHandler).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, AppSettingValueFinder>(typeof (AppSettingValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, ConnectionStringValueFinder>(typeof (ConnectionStringValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, ConfigurationValueFinder>(typeof (ConfigurationValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, NullValueFinder>(typeof(NullValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, EnvironmentValueFinder>(typeof(EnvironmentValueFinder).FullName, new PerContainerLifetime());

            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof (EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());
        }
    }
}
