using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Middleware;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Management;
using Jal.Router.Impl.Management.ShutdownWatcher;
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
            container.Register(Component.For<IChannelShuffler>().ImplementedBy<DefaultChannelShuffler>().LifestyleSingleton().Named(typeof(DefaultChannelShuffler).FullName));

            container.Register(Component.For<IChannelShuffler>().ImplementedBy<FisherYatesChannelShuffler>().LifestyleSingleton().Named(typeof(FisherYatesChannelShuffler).FullName));

            container.Register(Component.For<ILogger>().ImplementedBy<Impl.ConsoleLogger>().LifestyleSingleton());

            container.Register(Component.For<IRouter>().ImplementedBy<Impl.Inbound.Router>().LifestyleSingleton());

            container.Register(Component.For<IParameterProvider>().ImplementedBy<ParameterProvider>().LifestyleSingleton());

            container.Register(Component.For<IMessageRouter>().ImplementedBy<MessageRouter>().LifestyleSingleton());
            
            container.Register(Component.For<IHandlerMethodSelector>().ImplementedBy<HandlerMethodSelector>().LifestyleSingleton());

            container.Register(Component.For<IHandlerMethodExecutor>().ImplementedBy<HandlerMethodExecutor>().LifestyleSingleton());

            container.Register(Component.For<IEndPointProvider>().ImplementedBy<EndPointProvider>().LifestyleSingleton());

            container.Register(Component.For<IBus>().ImplementedBy<Bus>().LifestyleSingleton());

            container.Register(Component.For<ISender>().ImplementedBy<Sender>().LifestyleSingleton());

            container.Register(Component.For<IComponentFactory>().ImplementedBy<ComponentFactory>().LifestyleSingleton());

            container.Register(Component.For<IStartup>().ImplementedBy<Startup>().LifestyleSingleton());

            container.Register(Component.For<IShutdown>().ImplementedBy<Shutdown>().LifestyleSingleton());

            container.Register(Component.For<IConfiguration>().ImplementedBy<Configuration>().LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelCreator>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<Impl.StartupTask.StartupBeatLogger>().LifestyleSingleton().Named(typeof(Impl.StartupTask.StartupBeatLogger).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<ShutdownTask>().LifestyleSingleton().Named(typeof(ShutdownTask).FullName));

            container.Register(Component.For<IShutdownTask>().ImplementedBy<SenderShutdownTask>().LifestyleSingleton().Named(typeof(SenderShutdownTask).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<EndpointsInitializer>().LifestyleSingleton().Named(typeof(EndpointsInitializer).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<PointToPointChannelCreator>().LifestyleSingleton().Named(typeof(PointToPointChannelCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<PublishSubscriberChannelCreator>().LifestyleSingleton().Named(typeof(PublishSubscriberChannelCreator).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<RoutesInitializer>().LifestyleSingleton().Named(typeof(RoutesInitializer).FullName));

            container.Register(Component.For<IStartupTask>().ImplementedBy<RuntimeConfigurationLoader>().LifestyleSingleton().Named(typeof(RuntimeConfigurationLoader).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<ShutdownNullWatcher>().LifestyleSingleton().Named(typeof(ShutdownNullWatcher).FullName));

            container.Register(Component.For(typeof(IStartupTask)).ImplementedBy(typeof(ListenerLoader)).Named(typeof(ListenerLoader).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IStartupTask)).ImplementedBy(typeof(SenderLoader)).Named(typeof(SenderLoader).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IShutdownTask)).ImplementedBy(typeof(ListenerShutdownTask)).Named(typeof(ListenerShutdownTask).FullName).LifestyleSingleton());

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            container.Register(Component.For<ISagaStorageSearcher>().ImplementedBy<SagaStorageSearcher>().LifestyleSingleton());

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<PointToPointChannelMonitor>().LifestyleSingleton().Named(typeof(PointToPointChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<SubscriptionToPublishSubscribeChannelMonitor>().LifestyleSingleton().Named(typeof(SubscriptionToPublishSubscribeChannelMonitor).FullName));

            container.Register(Component.For<IMonitoringTask>().ImplementedBy<HeartBeatLogger>().LifestyleSingleton().Named(typeof(HeartBeatLogger).FullName));

            container.Register(Component.For<IMessageSerializer>().ImplementedBy<NullMessageSerializer>().LifestyleSingleton().Named(typeof(NullMessageSerializer).FullName));

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<NullMessageAdapter>().LifestyleSingleton().Named(typeof(NullMessageAdapter).FullName));

            container.Register(Component.For<IMessageStorage>().ImplementedBy<NullMessageStorage>().LifestyleSingleton().Named(typeof(NullMessageStorage).FullName));

            container.Register(Component.For<IShutdownWatcher>().ImplementedBy<ShutdownFileWatcher>().LifestyleSingleton().Named(typeof(ShutdownFileWatcher).FullName));

            container.Register(Component.For(typeof(IRouterInterceptor)).ImplementedBy(typeof(NullRouterInterceptor)).Named(typeof(NullRouterInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPointToPointChannel)).ImplementedBy(typeof(NullPointToPointChannel)).Named(typeof(NullPointToPointChannel).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IPublishSubscribeChannel)).ImplementedBy(typeof(NullPublishSubscribeChannel)).Named(typeof(NullPublishSubscribeChannel).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestReplyChannel)).ImplementedBy(typeof(NullRequestReplyChannel)).Named(typeof(NullRequestReplyChannel).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<NullChannelManager>().LifestyleSingleton().Named(typeof(NullChannelManager).FullName));

            container.Register(Component.For(typeof(ILogger<Beat>)).ImplementedBy(typeof(Impl.BeatLogger)).Named(typeof(Impl.BeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IBusInterceptor)).ImplementedBy(typeof(NullBusInterceptor)).Named(typeof(NullBusInterceptor).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(ISagaStorage)).ImplementedBy(typeof(NullSagaStorage)).Named(typeof(NullSagaStorage).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware<MessageContext>)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware<MessageContext>)).ImplementedBy(typeof(MessageExceptionHandler)).Named(typeof(MessageExceptionHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware<MessageContext>)).ImplementedBy(typeof(FirstMessageHandler)).Named(typeof(FirstMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware<MessageContext>)).ImplementedBy(typeof(MiddleMessageHandler)).Named(typeof(MiddleMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IMiddleware<MessageContext>)).ImplementedBy(typeof(LastMessageHandler)).Named(typeof(LastMessageHandler).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<DistributionHandler>().LifestyleSingleton().Named(typeof(DistributionHandler).FullName));

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<PointToPointHandler>().LifestyleSingleton().Named(typeof(PointToPointHandler).FullName));

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<PublishSubscribeHandler>().LifestyleSingleton().Named(typeof(PublishSubscribeHandler).FullName));

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<RequestReplyHandler>().LifestyleSingleton().Named(typeof(RequestReplyHandler).FullName));

            container.Register(Component.For(typeof(IValueFinder)).ImplementedBy(typeof(ConnectionStringValueFinder)).Named(typeof(ConnectionStringValueFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueFinder)).ImplementedBy(typeof(AppSettingValueFinder)).Named(typeof(AppSettingValueFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueFinder)).ImplementedBy(typeof(ConfigurationValueFinder)).Named(typeof(ConfigurationValueFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueFinder)).ImplementedBy(typeof(NullValueFinder)).Named(typeof(NullValueFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IValueFinder)).ImplementedBy(typeof(EnvironmentValueFinder)).Named(typeof(EnvironmentValueFinder).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(typeof(EmptyRouterConfigurationSource)).Named(typeof(EmptyRouterConfigurationSource).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IHost)).ImplementedBy(typeof(Host)).Named(typeof(Host).FullName).LifestyleSingleton());

            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn<IValueFinder>().WithServiceAllInterfaces());
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
