using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Installer
{
    public class RouterBuilder : AbstractRouterBuilder
    {
        private readonly IWindsorContainer _container;

        public RouterBuilder(IWindsorContainer container)
        {
            _container = container;
        }

        public override IRouterBuilder AddEntityStorage<TImplementation>()
        {
            _container.Register(Component.For<IEntityStorage>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddLogger<TImplementation, TInfo>()
        {
            _container.Register(Component.For<ILogger<TInfo>>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMessageAdapter<TImplementation>()
        {
            _container.Register(Component.For<IMessageAdapter>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMessageSerializer<TImplementation>()
        {
            _container.Register(Component.For<IMessageSerializer>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMessageStorage<TImplementation>()
        {

            _container.Register(Component.For<IMessageStorage>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMiddleware<TImplementation>()
        {
            _container.Register(Component.For<IAsyncMiddleware<MessageContext>>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddPointToPointChannel<TImplementation>()
        {
            _container.Register(Component.For<IPointToPointChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public override IRouterBuilder AddPublishSubscribeChannel<TImplementation>()
        {
            _container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public override IRouterBuilder AddSubscriptionToPublishSubscribeChannel<TImplementation>()
        {
            _container.Register(Component.For<ISubscriptionToPublishSubscribeChannel>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }

        public override IRouterBuilder AddSource<TImplementation>()
        {
            _container.Register(Component.For<IRouterConfigurationSource>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddStartupTask<TImplementation>()
        {
            _container.Register(Component.For<IStartupTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddShutdownTask<TImplementation>()
        {
            _container.Register(Component.For<IShutdownTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMonitoringTask<TImplementation>()
        {
            _container.Register(Component.For<IMonitoringTask>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddShutdownWatcher<TImplementation>()
        {
            _container.Register(Component.For<IShutdownWatcher>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddChannelShuffler<TImplementation>()
        {
            _container.Register(Component.For<IChannelShuffler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddRouteEntryMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IRouteEntryMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddRouteErrorMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IRouteErrorMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddRouteExitMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IRouteExitMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddRouterInterceptor<TImplementation>()
        {
            _container.Register(Component.For<IRouterInterceptor>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddBusEntryMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IBusEntryMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddBusErrorMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IBusErrorMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddBusExitMessageHandler<TImplementation>()
        {
            _container.Register(Component.For<IBusExitMessageHandler>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddBusInterceptor<TImplementation>()
        {
            _container.Register(Component.For<IBusInterceptor>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsSingleton<TService, TImplementation>()
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleSingleton());

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsTransient<TService, TImplementation>()
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName).LifestyleTransient());

            return this;
        }
    }
}
