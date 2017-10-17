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
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouter(this IServiceContainer container, Assembly[] sourceassemblies)
        {
            container.Register<IRouter, Impl.Inbound.Router>(new PerContainerLifetime());

            container.Register<IMessageRouter, MessageRouter>(new PerContainerLifetime());

            container.Register<IRouteMethodSelector, RouteMethodSelector>(new PerContainerLifetime());

            container.Register<IRouteProvider, RouteProvider>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IHandlerExecutor, HandlerExecutor>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IStartupConfiguration, ChannelStartupConfiguration>(typeof(ChannelStartupConfiguration).FullName, new PerContainerLifetime());

            container.Register<IMessageBodySerializer, NullMessageBodySerializer>(typeof(NullMessageBodySerializer).FullName, new PerContainerLifetime());

            container.Register<IMessageBodyAdapter, NullMessageBodyAdapter>(typeof(NullMessageBodyAdapter).FullName, new PerContainerLifetime());

            container.Register<IMessageMetadataAdapter, NullMessageMetadataAdapter>(typeof(NullMessageMetadataAdapter).FullName, new PerContainerLifetime());

            container.Register<IRouterLogger, NullRouterLogger>(typeof(NullRouterLogger).FullName, new PerContainerLifetime());

            container.Register<IRouterInterceptor, NullRouterInterceptor>(typeof(NullRouterInterceptor).FullName, new PerContainerLifetime());

            container.Register<IPointToPointChannel, NullPointToPointChannel>(typeof(NullPointToPointChannel).FullName, new PerContainerLifetime());

            container.Register<IPublishSubscribeChannel, NullPublishSubscribeChannel>(typeof(NullPublishSubscribeChannel).FullName, new PerContainerLifetime());

            container.Register<IChannelManager, NullChannelManager>(typeof(NullChannelManager).FullName, new PerContainerLifetime());

            container.Register<IBusInterceptor, NullBusInterceptor>(typeof(NullBusInterceptor).FullName, new PerContainerLifetime());

            container.Register<IBusLogger, NullBusLogger>(typeof(NullBusLogger).FullName, new PerContainerLifetime());

            container.Register<IStorage, NullStorage>(typeof(MessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, MessageHandler>(typeof(MessageHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, MessageExceptionHandler>(typeof(MessageExceptionHandler).FullName,new PerContainerLifetime());

            container.Register<IMiddleware, StartingMessageHandler>(typeof(StartingMessageHandler).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, NextMessageHandler>(typeof(NextMessageHandler).FullName, new PerContainerLifetime());

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
