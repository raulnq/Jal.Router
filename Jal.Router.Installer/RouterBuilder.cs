using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Installer
{
    public class RouterBuilder : IRouterBuilder
    {
        private readonly IWindsorContainer _container;

        public RouterBuilder(IWindsorContainer container)
        {
            _container = container;
        }

        public IRouterBuilder AddChannelResourceManager<TImplementation, TResource, TStatistics>() 
            where TImplementation : class, IChannelResourceManager<TResource, TStatistics>
            where TResource : Resource
        {
            _container.Register(Component.For<IChannelResourceManager<TResource, TStatistics>>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddEntityStorage<TImplementation>() where TImplementation : class, IEntityStorage
        {
            _container.Register(Component.For<IEntityStorage>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddLogger<TImplementation, TInfo>() where TImplementation : class, ILogger<TInfo>
        {
            _container.Register(Component.For<ILogger<TInfo>>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddMessageAdapter<TImplementation>() where TImplementation : class, IMessageAdapter
        {
            _container.Register(Component.For<IMessageAdapter>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddMessageSerializer<TImplementation>() where TImplementation : class, IMessageSerializer
        {
            _container.Register(Component.For<IMessageSerializer>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddMessageStorage<TImplementation>() where TImplementation : class, IMessageStorage
        {

            _container.Register(Component.For<IMessageStorage>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddMiddleware<TImplementation>() where TImplementation : class, IAsyncMiddleware<MessageContext>
        {
            _container.Register(Component.For<IAsyncMiddleware<MessageContext>>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddPointToPointChannel<TImplementation>() where TImplementation : class, IPointToPointChannel
        {
            _container.Register(Component.For<IPointToPointChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public IRouterBuilder AddPublishSubscribeChannel<TImplementation>() where TImplementation : class, IPublishSubscribeChannel
        {
            _container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public IRouterBuilder AddRequestReplyChannelFromPointToPointChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromPointToPointChannel
        {
            _container.Register(Component.For<IRequestReplyChannelFromPointToPointChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public IRouterBuilder AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
        {
            _container.Register(Component.For<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public IRouterBuilder AddSource<TImplementation>() where TImplementation : class, IRouterConfigurationSource
        {
            _container.Register(Component.For<IRouterConfigurationSource>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddStartupTask<TImplementation>() where TImplementation : class, IStartupTask
        {
            _container.Register(Component.For<IStartupTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddShutdownTask<TImplementation>() where TImplementation : class, IShutdownTask
        {
            _container.Register(Component.For<IShutdownTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddMonitoringTask<TImplementation>() where TImplementation : class, IMonitoringTask
        {
            _container.Register(Component.For<IMonitoringTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddShutdownWatcher<TImplementation>() where TImplementation : class, IShutdownWatcher
        {
            _container.Register(Component.For<IShutdownWatcher>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddChannelShuffler<TImplementation>() where TImplementation : class, IChannelShuffler
        {
            _container.Register(Component.For<IChannelShuffler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddRouteEntryMessageHandler<TImplementation>() where TImplementation : class, IRouteEntryMessageHandler
        {
            _container.Register(Component.For<IRouteEntryMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddRouteErrorMessageHandler<TImplementation>() where TImplementation : class, IRouteErrorMessageHandler
        {
            _container.Register(Component.For<IRouteErrorMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddRouteExitMessageHandler<TImplementation>() where TImplementation : class, IRouteExitMessageHandler
        {
            _container.Register(Component.For<IRouteExitMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddRouterInterceptor<TImplementation>() where TImplementation : class, IRouterInterceptor
        {
            _container.Register(Component.For<IRouterInterceptor>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddBusEntryMessageHandler<TImplementation>() where TImplementation : class, IBusEntryMessageHandler
        {
            _container.Register(Component.For<IBusEntryMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddBusErrorMessageHandler<TImplementation>() where TImplementation : class, IBusErrorMessageHandler
        {
            _container.Register(Component.For<IBusErrorMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddBusExitMessageHandler<TImplementation>() where TImplementation : class, IBusExitMessageHandler
        {
            _container.Register(Component.For<IBusExitMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public IRouterBuilder AddBusInterceptor<TImplementation>() where TImplementation : class, IBusInterceptor
        {
            _container.Register(Component.For<IBusInterceptor>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }
    }
}
