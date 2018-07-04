using System.Linq;
using System.Reflection;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Management;
using Jal.Router.Impl.Outbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model.Management;
using LightInject;
using IMiddleware = Jal.Router.Interface.Inbound.IMiddleware;

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

                        if (typeof(IValueSettingFinder).IsAssignableFrom(exportedType))
                        {
                            container.Register(typeof(IValueSettingFinder), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }

        public static void RegisterRouter(this IServiceContainer container, string shutdownfile = "")
        {
            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            container.Register<IHost, Host>(new PerContainerLifetime());

            container.Register<IRouter, Impl.Inbound.Router>(new PerContainerLifetime());

            container.Register<ISagaRouter, SagaRouter>(new PerContainerLifetime());

            container.Register<IMessageRouter, MessageRouter>(new PerContainerLifetime());

            container.Register<IHandlerMethodSelector, HandlerMethodSelector>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IHandlerMethodExecutor, HandlerMethodExecutor>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IShutdown, Shutdown>(new PerContainerLifetime());

            container.Register<IStartupTask, ChannelStartupTask>(typeof (ChannelStartupTask).FullName,new PerContainerLifetime());

            container.Register<IStartupTask, StartupTask>(typeof (StartupTask).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ShutdownTask>(typeof (ShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, HandlerAndEndpointStartupTask>(typeof (HandlerAndEndpointStartupTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ListenerStartupTask>(typeof (ListenerStartupTask).FullName,new PerContainerLifetime());

            container.Register<IShutdownTask, ListenerShutdownTask>(typeof (ListenerShutdownTask).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher, ShutdownNullWatcher>(typeof (ShutdownNullWatcher).FullName,new PerContainerLifetime());

            container.Register<IShutdownWatcher>(x => new ShutdownFileWatcher(shutdownfile),typeof (ShutdownFileWatcher).FullName, new PerContainerLifetime());

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<ISagaStorageFinder, SagaStorageFinder>(new PerContainerLifetime());

            container.Register<IMonitoringTask, PointToPointChannelMonitor>(typeof (PointToPointChannelMonitor).FullName,new PerContainerLifetime());

            container.Register<IMonitoringTask, SubscriptionToPublishSubscribeChannelMonitor>(typeof (SubscriptionToPublishSubscribeChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, HeartBeatMonitor>(typeof (HeartBeatMonitor).FullName, new PerContainerLifetime());

            container.Register<IMessageSerializer, NullMessageSerializer>(typeof (NullMessageSerializer).FullName,new PerContainerLifetime());

            container.Register<IMessageAdapter, NullMessageAdapter>(typeof (NullMessageAdapter).FullName,new PerContainerLifetime());

            container.Register<IMessageStorage, NullMessageStorage>(typeof(NullMessageStorage).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof (NullRouterInterceptor).FullName,new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof (NullPointToPointChannel).FullName,new PerContainerLifetime());

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof (NullPublishSubscribeChannel).FullName, new PerContainerLifetime());

            container.Register<IRequestReplyChannel, NullRequestReplyChannel>(typeof (NullRequestReplyChannel).FullName,new PerContainerLifetime());

            container.Register<IChannelManager, NullChannelManager>(typeof (NullChannelManager).FullName,new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof (NullBusInterceptor).FullName,new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, HeartBeatLogger>(typeof (HeartBeatLogger).FullName,new PerContainerLifetime());

            container.Register<ILogger<StartupBeat>, StartupBeatLogger>(typeof (StartupBeatLogger).FullName,new PerContainerLifetime());

            container.Register<ILogger<ShutdownBeat>, ShutdownBeatLogger>(typeof (ShutdownBeatLogger).FullName,new PerContainerLifetime());

            container.Register<ISagaStorage, NullSagaStorage>(typeof (NullSagaStorage).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, MessageHandler>(typeof (MessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, MessageExceptionHandler>(typeof (MessageExceptionHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, StartingMessageHandler>(typeof (StartingMessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, NextMessageHandler>(typeof (NextMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, EndingMessageHandler>(typeof (EndingMessageHandler).FullName,new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, PointToPointHandler>(typeof (PointToPointHandler).FullName,new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, PublishSubscribeHandler>(typeof (PublishSubscribeHandler).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, RequestReplyHandler>(typeof (RequestReplyHandler).FullName,new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, DistributionHandler>(typeof(DistributionHandler).FullName, new PerContainerLifetime());

            container.Register<IValueSettingFinder, AppSettingValueSettingFinder>(typeof (AppSettingValueSettingFinder).FullName, new PerContainerLifetime());

            container.Register<IValueSettingFinder, ConnectionStringValueSettingFinder>(typeof (ConnectionStringValueSettingFinder).FullName, new PerContainerLifetime());

#if NETSTANDARD2_0
            container.Register<IValueSettingFinder, ConfigurationValueSettingFinder>(typeof (ConfigurationValueSettingFinder).FullName, new PerContainerLifetime());
#endif
            container.Register<IValueSettingFinder, NullValueSettingFinder>(typeof(NullValueSettingFinder).FullName, new PerContainerLifetime());

            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof (EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());
        }
    }
}
