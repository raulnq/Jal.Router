﻿using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterBuilder : IRouterBuilder
    {
        public void Init()
        {
            AddPointToPointChannel<NullPointToPointChannel>();

            AddPublishSubscribeChannel<NullPublishSubscribeChannel>();

            AddSubscriptionToPublishSubscribeChannel<NullSubscriptionToPublishSubscribeChannel>();

            AddPointToPointChannel<FileSystemPointToPointChannel>();

            AddPublishSubscribeChannel<FileSystemPublishSubscribeChannel>();

            AddSubscriptionToPublishSubscribeChannel<FileSystemSubscriptionToPublishSubscribeChannel>();

            AddPointToPointChannel<InMemoryPointToPointChannel>();

            AddPublishSubscribeChannel<InMemoryPublishSubscribeChannel>();

            AddSubscriptionToPublishSubscribeChannel<InMemorySubscriptionToPublishSubscribeChannel>();

            AddShutdownTask<ChannelDestructor>();

            AddShutdownTask<ShutdownTask>();

            AddShutdownTask<SenderShutdownTask>();

            AddShutdownTask<ListenerShutdownTask>();

            AddStartupTask<StartupLogger>();

            AddStartupTask<EndpointValidator>();

            AddStartupTask<RouteValidator>();

            AddStartupTask<RuntimeLoader>();

            AddStartupTask<ListenerLoader>();

            AddStartupTask<SenderLoader>();

            AddMonitoringTask<StatisticMonitor>();

            AddMonitoringTask<MonitoringTask>();

            AddMonitoringTask<ListenerMonitor>();

            AddMonitoringTask<ListenerRestartMonitor>();

            AddMessageSerializer<NullMessageSerializer>();

            AddMessageAdapter<NullMessageAdapter>();

            AddMessageStorage<NullMessageStorage>();

            AddEntityStorage<NullEntityStorage>();

            AddEntityStorage<InMemoryEntityStorage>();

            AddLogger<StatisticLogger, Statistic>();

            AddLogger<BeatLogger, Beat>();

            AddMiddleware<ConsumerMiddleware>();

            AddMiddleware<RouterMiddleware>();

            AddMiddleware<InitialConsumerMiddleware>();

            AddMiddleware<MiddleConsumerMiddleware>();

            AddMiddleware<FinalConsumerMiddleware>();

            AddMiddleware<BusMiddleware>();

            AddMiddleware<ProducerMiddleware>();

            AddSource<EmptyRouterConfigurationSource>();

            AddShutdownWatcher<NullShutdownWatcher>();

            AddShutdownWatcher<FileShutdownWatcher>();

            AddShutdownWatcher<CtrlCShutdownWatcher>();

            AddShutdownWatcher<SignTermShutdownWatcher>();

            AddChannelShuffler<DefaultChannelShuffler>();

            AddChannelShuffler<FisherYatesChannelShuffler>();

            AddRouterInterceptor<NullRouterInterceptor>();

            AddBusInterceptor<NullBusInterceptor>();

            AddMessageAdapter<MessageAdapter>();

            AddRouteErrorMessageHandler<ForwardRouteErrorMessageHandler>();

            AddRouteErrorMessageHandler<RetryRouteErrorMessageHandler>();

            AddRouteEntryMessageHandler<ForwardRouteEntryMessageHandler>();

            AddRouteEntryMessageHandler<RouteEntryMessageHandler>();
        }

        public abstract IRouterBuilder AddEntityStorage<TImplementation>() where TImplementation : class, IEntityStorage;

        public abstract IRouterBuilder AddLogger<TImplementation, TInfo>() where TImplementation : class, ILogger<TInfo>;

        public abstract IRouterBuilder AddMessageAdapter<TImplementation>() where TImplementation : class, IMessageAdapter;

        public abstract IRouterBuilder AddMessageSerializer<TImplementation>() where TImplementation : class, IMessageSerializer;

        public abstract IRouterBuilder AddMessageStorage<TImplementation>() where TImplementation : class, IMessageStorage;

        public abstract IRouterBuilder AddMiddleware<TImplementation>() where TImplementation : class, IAsyncMiddleware<MessageContext>;

        public abstract IRouterBuilder AddPointToPointChannel<TImplementation>() where TImplementation : class, IPointToPointChannel;

        public abstract IRouterBuilder AddPublishSubscribeChannel<TImplementation>() where TImplementation : class, IPublishSubscribeChannel;

        public abstract IRouterBuilder AddSubscriptionToPublishSubscribeChannel<TImplementation>() where TImplementation : class, ISubscriptionToPublishSubscribeChannel;

        public abstract IRouterBuilder AddSource<TImplementation>() where TImplementation : class, IRouterConfigurationSource;

        public abstract IRouterBuilder AddStartupTask<TImplementation>() where TImplementation : class, IStartupTask;

        public abstract IRouterBuilder AddShutdownTask<TImplementation>() where TImplementation : class, IShutdownTask;

        public abstract IRouterBuilder AddMonitoringTask<TImplementation>() where TImplementation : class, IMonitoringTask;

        public abstract IRouterBuilder AddShutdownWatcher<TImplementation>() where TImplementation : class, IShutdownWatcher;

        public abstract IRouterBuilder AddChannelShuffler<TImplementation>() where TImplementation : class, IChannelShuffler;

        public abstract IRouterBuilder AddRouteEntryMessageHandler<TImplementation>() where TImplementation : class, IRouteEntryMessageHandler;

        public abstract IRouterBuilder AddRouteErrorMessageHandler<TImplementation>() where TImplementation : class, IRouteErrorMessageHandler;

        public abstract IRouterBuilder AddRouteExitMessageHandler<TImplementation>() where TImplementation : class, IRouteExitMessageHandler;

        public abstract IRouterBuilder AddRouterInterceptor<TImplementation>() where TImplementation : class, IRouterInterceptor;

        public abstract IRouterBuilder AddBusEntryMessageHandler<TImplementation>() where TImplementation : class, IBusEntryMessageHandler;

        public abstract IRouterBuilder AddBusErrorMessageHandler<TImplementation>() where TImplementation : class, IBusErrorMessageHandler;

        public abstract IRouterBuilder AddBusExitMessageHandler<TImplementation>() where TImplementation : class, IBusExitMessageHandler;

        public abstract IRouterBuilder AddBusInterceptor<TImplementation>() where TImplementation : class, IBusInterceptor;

        public abstract IRouterBuilder AddMessageHandlerAsSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        public abstract IRouterBuilder AddMessageHandlerAsTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
    }
}
