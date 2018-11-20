using System.Linq;
using System.Reflection;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Management;
using Jal.Router.Impl.MonitoringTask;
using Jal.Router.Impl.Outbound;
using Jal.Router.Impl.Outbound.ChannelShuffler;
using Jal.Router.Impl.Outbound.Middleware;
using Jal.Router.Impl.StartupTask;
using Jal.Router.Impl.ValueFinder;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouter(this IServiceContainer container, IRouterConfigurationSource[] sources,
            string shutdownfile = "")
        {
            RegisterRouter(container, shutdownfile);

            if (sources != null)
            {
                foreach (var source in sources)
                {
                    container.Register(typeof(IRouterConfigurationSource) , source.GetType(), source.GetType().FullName, new PerContainerLifetime());

                }
            }
        }


        public static void RegisterRouter(this IServiceContainer container, Assembly[] sourceassemblies, string shutdownfile="")
        {
            RegisterRouter(container, shutdownfile);

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

        public static void RegisterRouter(this IServiceContainer container, string shutdownfile = "")
        {
            container.Register<IChannelShuffler, DefaultChannelShuffler>(typeof(DefaultChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<IChannelShuffler, FisherYatesChannelShuffler>(typeof(FisherYatesChannelShuffler).FullName, new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

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

            container.Register<IStartupTask, PublishSubscriberChannelCreator>(typeof(PublishSubscriberChannelCreator).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ListenerLoader>(typeof (ListenerLoader).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, SenderLoader>(typeof(SenderLoader).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ListenerShutdownTask>(typeof (ListenerShutdownTask).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher, ShutdownNullWatcher>(typeof (ShutdownNullWatcher).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher>(x => new ShutdownFileWatcher(shutdownfile),typeof (ShutdownFileWatcher).FullName, new PerContainerLifetime());

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<ISagaStorageSearcher, SagaStorageSearcher>(new PerContainerLifetime());

            container.Register<IMonitoringTask, PointToPointChannelMonitor>(typeof (PointToPointChannelMonitor).FullName,new PerContainerLifetime());

            container.Register<IMonitoringTask, SubscriptionToPublishSubscribeChannelMonitor>(typeof (SubscriptionToPublishSubscribeChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, HeartBeatLogger>(typeof (HeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IMessageSerializer, NullMessageSerializer>(typeof (NullMessageSerializer).FullName,new PerContainerLifetime());

            container.Register<IMessageAdapter, NullMessageAdapter>(typeof (NullMessageAdapter).FullName,new PerContainerLifetime());

            container.Register<IMessageStorage, NullMessageStorage>(typeof(NullMessageStorage).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof (NullRouterInterceptor).FullName,new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof (NullPointToPointChannel).FullName,new PerContainerLifetime());

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof (NullPublishSubscribeChannel).FullName, new PerContainerLifetime());

            container.Register<IRequestReplyChannel, NullRequestReplyChannel>(typeof (NullRequestReplyChannel).FullName,new PerContainerLifetime());

            container.Register<IChannelManager, NullChannelManager>(typeof (NullChannelManager).FullName,new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof (NullBusInterceptor).FullName,new PerContainerLifetime());

            container.Register<ILogger<Beat>, BeatLogger>(typeof (BeatLogger).FullName,new PerContainerLifetime());

            container.Register<ISagaStorage, NullSagaStorage>(typeof (NullSagaStorage).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, MessageHandler>(typeof (MessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, MessageExceptionHandler>(typeof (MessageExceptionHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, FirstMessageHandler>(typeof (FirstMessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, MiddleMessageHandler>(typeof (MiddleMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, LastMessageHandler>(typeof (LastMessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, PointToPointHandler>(typeof (PointToPointHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, PublishSubscribeHandler>(typeof (PublishSubscribeHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, RequestReplyHandler>(typeof (RequestReplyHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, DistributionHandler>(typeof(DistributionHandler).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, AppSettingValueFinder>(typeof (AppSettingValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, ConnectionStringValueFinder>(typeof (ConnectionStringValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, ConfigurationValueFinder>(typeof (ConfigurationValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, NullValueFinder>(typeof(NullValueFinder).FullName, new PerContainerLifetime());

            container.Register<IValueFinder, EnvironmentValueFinder>(typeof(EnvironmentValueFinder).FullName, new PerContainerLifetime());

            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof (EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());
        }
    }
}
