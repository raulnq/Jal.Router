using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
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
using IMiddleware = Jal.Router.Interface.Inbound.IMiddleware;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _sourceassemblies;

        public RouterInstaller(Assembly[] sourceassemblies)
        {
            _sourceassemblies = sourceassemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRouter>().ImplementedBy<Impl.Inbound.Router>().LifestyleSingleton());

            container.Register(Component.For<IMessageRouter>().ImplementedBy<MessageRouter>().LifestyleSingleton());
            
            container.Register(Component.For<IRouteMethodSelector>().ImplementedBy<RouteMethodSelector>().LifestyleSingleton());

            container.Register(Component.For<IHandlerExecutor>().ImplementedBy<HandlerExecutor>().LifestyleSingleton());

            container.Register(Component.For<IRouteProvider>().ImplementedBy<RouteProvider>().LifestyleSingleton());

            container.Register(Component.For<IEndPointProvider>().ImplementedBy<EndPointProvider>().LifestyleSingleton());

            container.Register(Component.For<IBus>().ImplementedBy<Bus>().LifestyleSingleton());

            container.Register(Component.For<IComponentFactory>().ImplementedBy<ComponentFactory>().LifestyleSingleton());

            container.Register(Component.For<IStartup>().ImplementedBy<Startup>().LifestyleSingleton());

            container.Register(Component.For<IConfiguration>().ImplementedBy<Configuration>().LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<ChannelStartupTask>().LifestyleSingleton().Named(typeof(ChannelStartupTask).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<StartupTask>().LifestyleSingleton().Named(typeof(StartupTask).FullName));

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            container.Register(Component.For<IStorageFacade>().ImplementedBy<StorageFacade>().LifestyleSingleton());

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<PointToPointChannelMonitor>().LifestyleSingleton().Named(typeof(PointToPointChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelMonitor>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<HeartBeatMonitor>().LifestyleSingleton().Named(typeof(HeartBeatMonitor).FullName));

            container.Register(Component.For<IMessageBodySerializer>().ImplementedBy<NullMessageBodySerializer>().LifestyleSingleton().Named(typeof(NullMessageBodySerializer).FullName));

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<NullMessageAdapter>().LifestyleSingleton().Named(typeof(NullMessageAdapter).FullName));

            container.Register(Component.For(typeof(IRouterInterceptor)).ImplementedBy(typeof(NullRouterInterceptor)).Named(typeof(NullRouterInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPointToPointChannel)).ImplementedBy(typeof(NullPointToPointChannel)).Named(typeof(NullPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPublishSubscribeChannel)).ImplementedBy(typeof(NullPublishSubscribeChannel)).Named(typeof(NullPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<NullChannelManager>().LifestyleSingleton().Named(typeof(NullChannelManager).FullName));

            container.Register(Component.For(typeof(ILogger<HeartBeat>)).ImplementedBy(typeof(ConsoleHeartBeatLogger)).Named(typeof(ConsoleHeartBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<StartupBeat>)).ImplementedBy(typeof(ConsoleStartupBeatLogger)).Named(typeof(ConsoleStartupBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IBusInterceptor)).ImplementedBy(typeof(NullBusInterceptor)).Named(typeof(NullBusInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IStorage)).ImplementedBy(typeof(NullStorage)).Named(typeof(NullStorage).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(AppSettingsBasicAuthenticationHandler)).Named(typeof(AppSettingsBasicAuthenticationHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(MessageExceptionHandler)).Named(typeof(MessageExceptionHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(StartingMessageHandler)).Named(typeof(StartingMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(NextMessageHandler)).Named(typeof(NextMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<PointToPointHandler>().LifestyleSingleton().Named(typeof(PointToPointHandler).FullName));

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<PublishSubscribeHandler>().LifestyleSingleton().Named(typeof(PublishSubscribeHandler).FullName));

            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(ConnectionStringValueSettingFinder)).Named(typeof(ConnectionStringValueSettingFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(AppSettingValueSettingFinder)).Named(typeof(AppSettingValueSettingFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(typeof(EmptyRouterConfigurationSource)).Named(typeof(EmptyRouterConfigurationSource).FullName).LifestyleSingleton());
            
            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn<IValueSettingFinder>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn(typeof(IEndPointSettingFinder<>)).WithServiceAllInterfaces());
                }
            }
        }
    }
}
