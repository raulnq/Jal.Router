using System;
using Jal.Locator.Interface;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class RouterBuilder : IRouterBuilder, IRouterStartBuilder, IRouterEndBuilder
    {
        private IHandlerFactory _handlerFactory;

        private IRouteProvider _routeProvider;

        public IRouterBuilder UseServiceLocator(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException(nameof(serviceLocator));
            }

            _handlerFactory = new HandlerFactory(serviceLocator);

            return this;
        }

        public IRouterEndBuilder UseRouteConfigurationSource(IRouterConfigurationSource[] routerConfigurationSources)
        {
            if (routerConfigurationSources == null)
            {
                throw new ArgumentNullException(nameof(routerConfigurationSources));
            }

            _routeProvider = new RouteProvider(routerConfigurationSources);

            return this;
        }

        public IRouter<TMessage> Create<TMessage>()
        {
            //var result = new Router.Impl.Router(_handlerFactory, _routeProvider);

            return null;
        }
    }
}