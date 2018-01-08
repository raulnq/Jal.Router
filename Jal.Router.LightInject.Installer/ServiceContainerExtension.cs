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
        public static void RegisterRouter(this IServiceContainer container, Assembly[] sourceassemblies, string shutdownfile="")
        {
            container.Register<IHost, Host>(new PerContainerLifetime());

            container.Register<IRouter, Impl.Inbound.Router>(new PerContainerLifetime());

            container.Register<IMessageRouter, MessageRouter>(new PerContainerLifetime());

            container.Register<IRouteMethodSelector, RouteMethodSelector>(new PerContainerLifetime());

            container.Register<IChannelPathBuilder, ChannelPathBuilder>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IHandlerExecutor, HandlerExecutor>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IShutdown, Shutdown>(new PerContainerLifetime());

            container.Register<IStartupTask, ChannelStartupTask>(typeof(ChannelStartupTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, StartupTask>(typeof(StartupTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ConfigurationSanityCheckStartupTask>(typeof(ConfigurationSanityCheckStartupTask).FullName, new PerContainerLifetime());

            container.Register<IStartupTask, ListenerStartupTask>(typeof(ListenerStartupTask).FullName, new PerContainerLifetime());

            container.Register<IShutdownTask, ListenerShutdownTask>(typeof(ListenerShutdownTask).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher, ShutdownNullWatcher>(typeof(ShutdownNullWatcher).FullName, new PerContainerLifetime());

            container.Register<IShutdownWatcher>(x=> new ShutdownFileWatcher(shutdownfile),typeof(ShutdownFileWatcher).FullName, new PerContainerLifetime());

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<IStorageFacade, StorageFacade>(new PerContainerLifetime());

            container.Register<IMonitoringTask, PointToPointChannelMonitor>(typeof(PointToPointChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, SubscriptionToPublishSubscribeChannelMonitor>(typeof(SubscriptionToPublishSubscribeChannelMonitor).FullName, new PerContainerLifetime());

            container.Register<IMonitoringTask, HeartBeatMonitor>(typeof(HeartBeatMonitor).FullName, new PerContainerLifetime());

            container.Register<IMessageBodySerializer, NullMessageBodySerializer>(typeof(NullMessageBodySerializer).FullName, new PerContainerLifetime());

            container.Register<IMessageAdapter, NullMessageAdapter>(typeof(NullMessageAdapter).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof(NullRouterInterceptor).FullName, new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof(NullPointToPointChannel).FullName, new PerContainerLifetime());

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof(NullPublishSubscribeChannel).FullName, new PerContainerLifetime());

            container.Register<IRequestReplyChannel, NullRequestReplyChannel>(typeof(NullRequestReplyChannel).FullName, new PerContainerLifetime());

            container.Register<IChannelManager, NullChannelManager>(typeof(NullChannelManager).FullName, new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof(NullBusInterceptor).FullName, new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, ConsoleHeartBeatLogger>(typeof(ConsoleHeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<StartupBeat>, ConsoleStartupBeatLogger>(typeof(ConsoleStartupBeatLogger).FullName, new PerContainerLifetime());

            container.Register<IStorage, NullStorage>(typeof(NullStorage).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, MessageHandler>(typeof(MessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, AppSettingsBasicAuthenticationHandler>(typeof(AppSettingsBasicAuthenticationHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, MessageExceptionHandler>(typeof(MessageExceptionHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, StartingMessageHandler>(typeof(StartingMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, NextMessageHandler>(typeof(NextMessageHandler).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, PointToPointHandler>(typeof(PointToPointHandler).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, PublishSubscribeHandler>(typeof(PublishSubscribeHandler).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, RequestReplyHandler>(typeof(RequestReplyHandler).FullName, new PerContainerLifetime());

            container.Register<IValueSettingFinder, AppSettingValueSettingFinder>(typeof(AppSettingValueSettingFinder).FullName ,new PerContainerLifetime());

            container.Register<IValueSettingFinder, ConnectionStringValueSettingFinder>(typeof(ConnectionStringValueSettingFinder).FullName, new PerContainerLifetime());
          
            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof(EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());

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

                        if (exportedType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEndPointSettingFinder<>)))
                        {
                            var argument = exportedType.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEndPointSettingFinder<>)).GetGenericArguments();

                            var type = typeof(IEndPointSettingFinder<>);

                            var genericType = type.MakeGenericType(argument);

                            container.Register(genericType, exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }
    }
}
