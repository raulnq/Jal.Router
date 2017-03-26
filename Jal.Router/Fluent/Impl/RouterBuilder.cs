using System;
using Jal.Locator.Interface;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class RouterBuilder : IRouterBuilder, IRouterStartBuilder, IInterceptorRouterBuilder
    {
        private IHandlerFactory _handlerFactory;

        private IRouterInterceptor _routerInterceptor;

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

        public IInterceptorRouterBuilder UseRouteConfigurationSource(IRouterConfigurationSource[] routerConfigurationSources)
        {
            if (routerConfigurationSources == null)
            {
                throw new ArgumentNullException(nameof(routerConfigurationSources));
            }

            _routeProvider = new RouteProvider(routerConfigurationSources);

            return this;
        }


        public IRouter Create
        {
            get
            {
                var result = new Router.Impl.Router(_handlerFactory, _routeProvider);

                if (_routerInterceptor != null)
                {
                    result.Interceptor = _routerInterceptor;
                }

                return result;
            }
            
        }

        public IRouterBuilder UseInterceptor(IRouterInterceptor routerInterceptor)
        {
            if (routerInterceptor == null)
            {
                throw new ArgumentNullException(nameof(routerInterceptor));
            }
            _routerInterceptor = routerInterceptor;
            return this;
        }
    }
}