using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterBuilder
    {
        IRouterBuilder AddSource<TImplementation>() where TImplementation : class, IRouterConfigurationSource;

        IRouterBuilder AddMiddleware<TImplementation>() where TImplementation : class, IAsyncMiddleware<MessageContext>;

        IRouterBuilder AddLogger<TImplementation, TInfo>() where TImplementation : class, ILogger<TInfo>;

        IRouterBuilder AddMessageStorage<TImplementation>() where TImplementation : class, IMessageStorage;

        IRouterBuilder AddEntityStorage<TImplementation>() where TImplementation : class, IEntityStorage;

        IRouterBuilder AddStartupTask<TImplementation>() where TImplementation : class, IStartupTask;

        IRouterBuilder AddShutdownTask<TImplementation>() where TImplementation : class, IShutdownTask;

        IRouterBuilder AddMonitoringTask<TImplementation>() where TImplementation : class, IMonitoringTask;

        IRouterBuilder AddMessageSerializer<TImplementation>() where TImplementation : class, IMessageSerializer;

        IRouterBuilder AddPointToPointChannel<TImplementation>() where TImplementation : class, IPointToPointChannel;

        IRouterBuilder AddMessageAdapter<TImplementation>() where TImplementation : class, IMessageAdapter;

        IRouterBuilder AddPublishSubscribeChannel<TImplementation>() where TImplementation : class, IPublishSubscribeChannel;

        IRouterBuilder AddRequestReplyChannelFromPointToPointChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromPointToPointChannel;

        IRouterBuilder AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel;

        IRouterBuilder AddResourceManager<TImplementation>() where TImplementation : class, IResourceManager;

        IRouterBuilder AddShutdownWatcher<TImplementation>() where TImplementation : class, IShutdownWatcher;

        IRouterBuilder AddChannelShuffler<TImplementation>() where TImplementation : class, IChannelShuffler;

        IRouterBuilder AddRouteEntryMessageHandler<TImplementation>() where TImplementation : class, IRouteEntryMessageHandler;

        IRouterBuilder AddRouteErrorMessageHandler<TImplementation>() where TImplementation : class, IRouteErrorMessageHandler;

        IRouterBuilder AddRouteExitMessageHandler<TImplementation>() where TImplementation : class, IRouteExitMessageHandler;

        IRouterBuilder AddRouterInterceptor<TImplementation>() where TImplementation : class, IRouterInterceptor;

        IRouterBuilder AddBusEntryMessageHandler<TImplementation>() where TImplementation : class, IBusEntryMessageHandler;

        IRouterBuilder AddBusErrorMessageHandler<TImplementation>() where TImplementation : class, IBusErrorMessageHandler;

        IRouterBuilder AddBusExitMessageHandler<TImplementation>() where TImplementation : class, IBusExitMessageHandler;

        IRouterBuilder AddBusInterceptor<TImplementation>() where TImplementation : class, IBusInterceptor;
    }
}