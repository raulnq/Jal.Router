using Jal.ChainOfResponsability;
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

            AddRequestReplyChannelFromPointToPointChannel<NullRequestReplyChannelFromPointToPointChannel>();

            AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            AddPointToPointChannel<FileSystemPointToPointChannel>();

            AddPublishSubscribeChannel<FileSystemPublishSubscribeChannel>();

            AddRequestReplyChannelFromPointToPointChannel<FileSystemRequestReplyFromPointToPointChannel>();

            AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>();

            AddPointToPointChannel<InMemoryPointToPointChannel>();

            AddPublishSubscribeChannel<InMemoryPublishSubscribeChannel>();

            AddRequestReplyChannelFromPointToPointChannel<InMemoryRequestReplyFromPointToPointChannel>();

            AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>();

            AddShutdownTask<ResourceDestructor>();

            AddShutdownTask<ShutdownTask>();

            AddShutdownTask<SenderShutdownTask>();

            AddShutdownTask<ListenerShutdownTask>();

            AddStartupTask<ResourceValidator>();

            AddStartupTask<StartupBeatLogger>();

            AddStartupTask<EndpointValidator>();

            AddStartupTask<ResourceCreator>();

            AddStartupTask<RouteValidator>();

            AddStartupTask<RuntimeLoader>();

            AddStartupTask<ListenerLoader>();

            AddStartupTask<SenderLoader>();

            AddMonitoringTask<StatisticMonitor>();

            AddMonitoringTask<HeartBeatLogger>();

            AddMonitoringTask<ListenerMonitor>();

            AddMonitoringTask<ListenerRestartMonitor>();

            AddMessageSerializer<NullMessageSerializer>();

            AddMessageAdapter<NullMessageAdapter>();

            AddMessageStorage<NullMessageStorage>();

            AddEntityStorage<NullEntityStorage>();

            AddEntityStorage<InMemoryEntityStorage>();

            AddResourceManager<NullResourceManager>();

            AddResourceManager<FileSystemPointToPointlResourceManager>();

            AddResourceManager<FileSystemPublishSubscribeResourceManager>();

            AddResourceManager<FileSystemSubscriptionToPublishSubscribeResourceManager>();

            AddResourceManager<InMemoryPointToPointResourceManager>();

            AddResourceManager<InMemoryPublishSubscribeResourceManager>();

            AddResourceManager<InMemorySubscriptionToPublishSubscribeResourceManager>();

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

        public abstract IRouterBuilder AddResourceManager<TImplementation>() where TImplementation : class, IResourceManager;

        public abstract IRouterBuilder AddEntityStorage<TImplementation>() where TImplementation : class, IEntityStorage;

        public abstract IRouterBuilder AddLogger<TImplementation, TInfo>() where TImplementation : class, ILogger<TInfo>;

        public abstract IRouterBuilder AddMessageAdapter<TImplementation>() where TImplementation : class, IMessageAdapter;

        public abstract IRouterBuilder AddMessageSerializer<TImplementation>() where TImplementation : class, IMessageSerializer;

        public abstract IRouterBuilder AddMessageStorage<TImplementation>() where TImplementation : class, IMessageStorage;

        public abstract IRouterBuilder AddMiddleware<TImplementation>() where TImplementation : class, IAsyncMiddleware<MessageContext>;

        public abstract IRouterBuilder AddPointToPointChannel<TImplementation>() where TImplementation : class, IPointToPointChannel;

        public abstract IRouterBuilder AddPublishSubscribeChannel<TImplementation>() where TImplementation : class, IPublishSubscribeChannel;

        public abstract IRouterBuilder AddRequestReplyChannelFromPointToPointChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromPointToPointChannel;

        public abstract IRouterBuilder AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel;

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
    }
}
