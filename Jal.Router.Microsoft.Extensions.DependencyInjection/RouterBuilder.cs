using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Jal.Router.Microsoft.Extensions.DependencyInjection
{
    public class RouterBuilder : AbstractRouterBuilder
    {
        private readonly IServiceCollection _container;

        public RouterBuilder(IServiceCollection container)
        {
            _container = container;
        }

        public override IRouterBuilder AddEntityStorage<TImplementation>()
        {
            _container.AddSingleton<IEntityStorage, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddLogger<TImplementation, TInfo>()
        {
            _container.AddSingleton<ILogger<TInfo>, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMessageAdapter<TImplementation>()
        {
            _container.AddSingleton<IMessageAdapter, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMessageSerializer<TImplementation>()
        {
            _container.AddSingleton<IMessageSerializer, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMessageStorage<TImplementation>()
        {
            _container.AddSingleton<IMessageStorage, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMiddleware<TImplementation>()
        {
            _container.AddSingleton<IAsyncMiddleware<MessageContext>, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddPointToPointChannel<TImplementation>()
        {
            _container.AddTransient<IPointToPointChannel, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddPublishSubscribeChannel<TImplementation>()
        {
            _container.AddTransient<IPublishSubscribeChannel, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddSubscriptionToPublishSubscribeChannel<TImplementation>()
        {
            _container.AddTransient<ISubscriptionToPublishSubscribeChannel, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddSource<TImplementation>()
        {
            _container.AddSingleton<IRouterConfigurationSource, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddStartupTask<TImplementation>()
        {
            _container.AddSingleton<IStartupTask, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddShutdownTask<TImplementation>()
        {
            _container.AddSingleton<IShutdownTask, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMonitoringTask<TImplementation>()
        {
            _container.AddSingleton<IMonitoringTask, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddShutdownWatcher<TImplementation>()
        {
            _container.AddSingleton<IShutdownWatcher, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddChannelShuffler<TImplementation>()
        {
            _container.AddSingleton<IChannelShuffler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddRouteEntryMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IRouteEntryMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddRouteErrorMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IRouteErrorMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddRouteExitMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IRouteExitMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddRouterInterceptor<TImplementation>()
        {
            _container.AddSingleton<IRouterInterceptor, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddBusEntryMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IBusEntryMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddBusErrorMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IBusErrorMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddBusExitMessageHandler<TImplementation>()
        {
            _container.AddSingleton<IBusExitMessageHandler, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddBusInterceptor<TImplementation>()
        {
            _container.AddSingleton<IBusInterceptor, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsSingleton<TService, TImplementation>()
        {
            _container.AddSingleton<TService, TImplementation>();

            return this;
        }

        public override IRouterBuilder AddMessageHandlerAsTransient<TService, TImplementation>()
        {
            _container.AddTransient<TService, TImplementation>();

            return this;
        }
    }
}
