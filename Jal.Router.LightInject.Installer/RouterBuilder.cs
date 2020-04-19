using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;
using System;

namespace Jal.Router.LightInject.Installer
{
    public class RouterBuilder : IRouterBuilder
    {
        private readonly IServiceContainer _container;

        public RouterBuilder(IServiceContainer container)
        {
            _container = container;
        }

        public IRouterBuilder AddChannelResourceManager<TImplementation, TResource, TStatistics>() 
            where TImplementation : class, IChannelResourceManager<TResource, TStatistics>
            where TResource : ChannelResource
        {
            _container.Register<IChannelResourceManager<TResource, TStatistics>, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddEntityStorage<TImplementation>() where TImplementation : class, IEntityStorage
        {
            _container.Register<IEntityStorage, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddLogger<TImplementation, TInfo>() where TImplementation : class, ILogger<TInfo>
        {
            _container.Register<ILogger<TInfo>, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddMessageAdapter<TImplementation>() where TImplementation : class, IMessageAdapter
        {
            _container.Register<IMessageAdapter, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddMessageSerializer<TImplementation>() where TImplementation : class, IMessageSerializer
        {
            _container.Register<IMessageSerializer, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddMessageStorage<TImplementation>() where TImplementation : class, IMessageStorage
        {
            _container.Register<IMessageStorage, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddMiddleware<TImplementation>() where TImplementation : class, IAsyncMiddleware<MessageContext>
        {
            _container.Register<IAsyncMiddleware<MessageContext>, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddPointToPointChannel<TImplementation>() where TImplementation : class, IPointToPointChannel
        {
            _container.Register<IPointToPointChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public IRouterBuilder AddPublishSubscribeChannel<TImplementation>() where TImplementation : class, IPublishSubscribeChannel
        {
            _container.Register<IPublishSubscribeChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public IRouterBuilder AddRequestReplyChannelFromPointToPointChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromPointToPointChannel
        {
            _container.Register<IRequestReplyChannelFromPointToPointChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public IRouterBuilder AddRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TImplementation>() where TImplementation : class, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
        {
            _container.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, TImplementation>(typeof(TImplementation).FullName);

            return this;
        }

        public IRouterBuilder AddSource<TImplementation>() where TImplementation : class, IRouterConfigurationSource
        {
            _container.Register<IRouterConfigurationSource, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddStartupTask<TImplementation>() where TImplementation : class, IStartupTask
        {
            _container.Register<IStartupTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddShutdownTask<TImplementation>() where TImplementation : class, IShutdownTask
        {
            _container.Register<IShutdownTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddMonitoringTask<TImplementation>() where TImplementation : class, IMonitoringTask
        {
            _container.Register<IMonitoringTask, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddShutdownWatcher<TImplementation>() where TImplementation : class, IShutdownWatcher
        {
            _container.Register<IShutdownWatcher, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddChannelShuffler<TImplementation>() where TImplementation : class, IChannelShuffler
        {
            _container.Register<IChannelShuffler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddRouteEntryMessageHandler<TImplementation>() where TImplementation : class, IRouteEntryMessageHandler
        {
            _container.Register<IRouteEntryMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddRouteErrorMessageHandler<TImplementation>() where TImplementation : class, IRouteErrorMessageHandler
        {
            _container.Register<IRouteErrorMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddRouteExitMessageHandler<TImplementation>() where TImplementation : class, IRouteExitMessageHandler
        {
            _container.Register<IRouteExitMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddRouterInterceptor<TImplementation>() where TImplementation : class, IRouterInterceptor
        {
            _container.Register<IRouterInterceptor, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddBusEntryMessageHandler<TImplementation>() where TImplementation : class, IBusEntryMessageHandler
        {
            _container.Register<IBusEntryMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddBusErrorMessageHandler<TImplementation>() where TImplementation : class, IBusErrorMessageHandler
        {
            _container.Register<IBusErrorMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddBusExitMessageHandler<TImplementation>() where TImplementation : class, IBusExitMessageHandler
        {
            _container.Register<IBusExitMessageHandler, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IRouterBuilder AddBusInterceptor<TImplementation>() where TImplementation : class, IBusInterceptor
        {
            _container.Register<IBusInterceptor, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }
    }
}
