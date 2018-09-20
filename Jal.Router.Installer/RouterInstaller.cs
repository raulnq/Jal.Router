﻿using System.Reflection;
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

        private readonly string _shutdownfile;

        private readonly IRouterConfigurationSource[] _sources;

        public RouterInstaller(string shutdownfile = "")
        {
            _shutdownfile = shutdownfile;
        }

        public RouterInstaller(Assembly[] sourceassemblies, string shutdownfile = "")
        {
            _sourceassemblies = sourceassemblies;

            _shutdownfile = shutdownfile;
        }

        public RouterInstaller(IRouterConfigurationSource[] sources, string shutdownfile = "")
        {
            _sources = sources;

            _shutdownfile = shutdownfile;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IChannelShuffler>().ImplementedBy<DefaultChannelShuffler>().LifestyleSingleton().Named(typeof(DefaultChannelShuffler).FullName));

            container.Register(Component.For<IChannelShuffler>().ImplementedBy<FisherYatesChannelShuffler>().LifestyleSingleton().Named(typeof(FisherYatesChannelShuffler).FullName));

            container.Register(Component.For<ILogger>().ImplementedBy<Impl.ConsoleLogger>().LifestyleSingleton());

            container.Register(Component.For<Interface.Inbound.IPipeline>().ImplementedBy<Impl.Inbound.Pipeline>().LifestyleSingleton());

            container.Register(Component.For<Interface.Outbound.IPipeline>().ImplementedBy<Impl.Outbound.Pipeline>().LifestyleSingleton());

            container.Register(Component.For<IRouter>().ImplementedBy<Impl.Inbound.Router>().LifestyleSingleton());

            container.Register(Component.For<ISagaExecutionCoordinator>().ImplementedBy<SagaExecutionCoordinator>().LifestyleSingleton());

            container.Register(Component.For<IMessageRouter>().ImplementedBy<MessageRouter>().LifestyleSingleton());
            
            container.Register(Component.For<IHandlerMethodSelector>().ImplementedBy<HandlerMethodSelector>().LifestyleSingleton());

            container.Register(Component.For<IHandlerMethodExecutor>().ImplementedBy<HandlerMethodExecutor>().LifestyleSingleton());

            container.Register(Component.For<IEndPointProvider>().ImplementedBy<EndPointProvider>().LifestyleSingleton());

            container.Register(Component.For<IBus>().ImplementedBy<Bus>().LifestyleSingleton());

            container.Register(Component.For<IComponentFactory>().ImplementedBy<ComponentFactory>().LifestyleSingleton());

            container.Register(Component.For<IStartup>().ImplementedBy<Startup>().LifestyleSingleton());

            container.Register(Component.For<IShutdown>().ImplementedBy<Shutdown>().LifestyleSingleton());

            container.Register(Component.For<IConfiguration>().ImplementedBy<Configuration>().LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<ChannelStartupTask>().LifestyleSingleton().Named(typeof(ChannelStartupTask).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<StartupTask>().LifestyleSingleton().Named(typeof(StartupTask).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<ShutdownTask>().LifestyleSingleton().Named(typeof(ShutdownTask).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<HandlerAndEndpointStartupTask>().LifestyleSingleton().Named(typeof(HandlerAndEndpointStartupTask).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<ShutdownNullWatcher>().LifestyleSingleton().Named(typeof(ShutdownNullWatcher).FullName));

            container.Register(Component.For(typeof(IStartupTask)).ImplementedBy(typeof(ListenerStartupTask)).Named(typeof(ListenerStartupTask).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IShutdownTask)).ImplementedBy(typeof(ListenerShutdownTask)).Named(typeof(ListenerShutdownTask).FullName).LifestyleSingleton());

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            container.Register(Component.For<ISagaStorageSearcher>().ImplementedBy<SagaStorageSearcher>().LifestyleSingleton());

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<PointToPointChannelMonitor>().LifestyleSingleton().Named(typeof(PointToPointChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelMonitor>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<HeartBeatMonitor>().LifestyleSingleton().Named(typeof(HeartBeatMonitor).FullName));

            container.Register(Component.For<IMessageSerializer>().ImplementedBy<NullMessageSerializer>().LifestyleSingleton().Named(typeof(NullMessageSerializer).FullName));

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<NullMessageAdapter>().LifestyleSingleton().Named(typeof(NullMessageAdapter).FullName));

            container.Register(Component.For<IMessageStorage>().ImplementedBy<NullMessageStorage>().LifestyleSingleton().Named(typeof(NullMessageStorage).FullName));

            container.Register(Component.For<IShutdownWatcher>().Instance(new ShutdownFileWatcher(_shutdownfile)).LifestyleSingleton().Named(typeof(ShutdownFileWatcher).FullName));

            container.Register(Component.For(typeof(IRouterInterceptor)).ImplementedBy(typeof(NullRouterInterceptor)).Named(typeof(NullRouterInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPointToPointChannel)).ImplementedBy(typeof(NullPointToPointChannel)).Named(typeof(NullPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPublishSubscribeChannel)).ImplementedBy(typeof(NullPublishSubscribeChannel)).Named(typeof(NullPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestReplyChannel)).ImplementedBy(typeof(NullRequestReplyChannel)).Named(typeof(NullRequestReplyChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<NullChannelManager>().LifestyleSingleton().Named(typeof(NullChannelManager).FullName));

            container.Register(Component.For(typeof(ILogger<HeartBeat>)).ImplementedBy(typeof(HeartBeatLogger)).Named(typeof(HeartBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<StartupBeat>)).ImplementedBy(typeof(StartupBeatLogger)).Named(typeof(StartupBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ILogger<ShutdownBeat>)).ImplementedBy(typeof(ShutdownBeatLogger)).Named(typeof(ShutdownBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IBusInterceptor)).ImplementedBy(typeof(NullBusInterceptor)).Named(typeof(NullBusInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ISagaStorage)).ImplementedBy(typeof(NullSagaStorage)).Named(typeof(NullSagaStorage).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(MessageExceptionHandler)).Named(typeof(MessageExceptionHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(FirstMessageHandler)).Named(typeof(FirstMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(MiddleMessageHandler)).Named(typeof(MiddleMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware)).ImplementedBy(typeof(LastMessageHandler)).Named(typeof(LastMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<DistributionHandler>().LifestyleSingleton().Named(typeof(DistributionHandler).FullName));

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<PointToPointHandler>().LifestyleSingleton().Named(typeof(PointToPointHandler).FullName));

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<PublishSubscribeHandler>().LifestyleSingleton().Named(typeof(PublishSubscribeHandler).FullName));

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<RequestReplyHandler>().LifestyleSingleton().Named(typeof(RequestReplyHandler).FullName));

            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(ConnectionStringValueSettingFinder)).Named(typeof(ConnectionStringValueSettingFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(AppSettingValueSettingFinder)).Named(typeof(AppSettingValueSettingFinder).FullName).LifestyleSingleton());

#if NETSTANDARD2_0
            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(ConfigurationValueSettingFinder)).Named(typeof(ConfigurationValueSettingFinder).FullName).LifestyleSingleton());
#endif
            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(NullValueSettingFinder)).Named(typeof(NullValueSettingFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(EnvironmentSettingFinder)).Named(typeof(EnvironmentSettingFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(typeof(EmptyRouterConfigurationSource)).Named(typeof(EmptyRouterConfigurationSource).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IHost)).ImplementedBy(typeof(Host)).Named(typeof(Host).FullName).LifestyleSingleton());

            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn<IValueSettingFinder>().WithServiceAllInterfaces());
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
