using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public class RouterBuilder : AbstractRouterBuilder
    {
        private readonly IServiceContainer _container;

        public RouterBuilder(IServiceContainer container)
        {
            _container = container;
        }

        public override IRouterBuilder AddResource<TImplementation>() 
        {
            _container.Register<IResource, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddEntityStorage<TImplementation>()
        {
            _container.Register<IEntityStorage, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddLogger<TImplementation, TInfo>()
        {
            _container.Register<ILogger<TInfo>, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMessageAdapter<TImplementation>()
        {
            _container.Register<IMessageAdapter, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMessageSerializer<TImplementation>()
        {
            _container.Register<IMessageSerializer, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMessageStorage<TImplementation>()
        {
            _container.Register<IMessageStorage, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMiddleware<TImplementation>()
        {
            _container.Register<IAsyncMiddleware<MessageContext>, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddPointToPointChannel<TImplementation>()
        {
            _container.Register<IPointToPointChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public override IRouterBuilder AddPublishSubscribeChannel<TImplementation>()
        {
            _container.Register<IPublishSubscribeChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public override IRouterBuilder AddRequestReplyChannelFromPointToPointChannel<TImplementation>()
        {
            _container.Register<IRequestReplyChannelFromPointToPointChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public override IRouterBuilder AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TImplementation>()
        {
            _container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public override IRouterBuilder AddSource<TImplementation>()
        {
            _container.Register<IRouterConfigurationSource, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddStartupTask<TImplementation>()
        {
            _container.Register<IStartupTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddShutdownTask<TImplementation>()
        {
            _container.Register<IShutdownTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMonitoringTask<TImplementation>()
        {
            _container.Register<IMonitoringTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddShutdownWatcher<TImplementation>()
        {
            _container.Register<IShutdownWatcher, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddChannelShuffler<TImplementation>()
        {
            _container.Register<IChannelShuffler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddRouteEntryMessageHandler<TImplementation>()
        {
            _container.Register<IRouteEntryMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddRouteErrorMessageHandler<TImplementation>()
        {
            _container.Register<IRouteErrorMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddRouteExitMessageHandler<TImplementation>()
        {
            _container.Register<IRouteExitMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddRouterInterceptor<TImplementation>()
        {
            _container.Register<IRouterInterceptor, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddBusEntryMessageHandler<TImplementation>()
        {
            _container.Register<IBusEntryMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddBusErrorMessageHandler<TImplementation>()
        {
            _container.Register<IBusErrorMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddBusExitMessageHandler<TImplementation>()
        {
            _container.Register<IBusExitMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddBusInterceptor<TImplementation>()
        {
            _container.Register<IBusInterceptor, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsSingleton<TService, TImplementation>()
        {
            _container.Register<TService, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsTransient<TService, TImplementation>()
        {
            _container.Register<TService, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }
    }
}
