using System;
using Jal.Factory.Interface;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Settings.Impl;

namespace Jal.Router.Fluent.Impl
{
    public class RequestRouterFluentBuilder : IRequestRouterFluentBuilder, IRequestRouterStartFluentBuilder
    {
        private IRequestRouter _requestRouter;

        private IRouteProvider _routeProvider;

        private IRequestRouterInterceptor _requestRouterInterceptor;

        public IRequestRouterFluentBuilder UseRouteProvider(IObjectFactory objectFactory)
        {
            if (objectFactory == null)
            {
                throw new ArgumentNullException("objectFactory");
            }
            _routeProvider = new RouteProvider(new RequestSenderSource(objectFactory), new EndPointSource(objectFactory,SettingsExtractor.Builder.Create));
            return this;
        }

        public IRequestRouterEndFluentBuilder UseRequestRouter(IRequestRouter requestRouter)
        {
            if (requestRouter == null)
            {
                throw new ArgumentNullException("requestRouter");
            }
            _requestRouter = requestRouter;
            return this;
        }

        public IRequestRouter Create
        {
            get
            {
                if (_requestRouter != null)
                {
                    return _requestRouter;
                }

                var result = new RequestRouter(_routeProvider);

                if (_requestRouterInterceptor != null)
                {
                    result.Interceptor = _requestRouterInterceptor;
                }

                return result;
            }
            
        }

        public IRequestRouterFluentBuilder UseInterceptor(IRequestRouterInterceptor requestRouterInterceptor)
        {
            if (requestRouterInterceptor == null)
            {
                throw new ArgumentNullException("requestRouterInterceptor");
            }
            _requestRouterInterceptor = requestRouterInterceptor;
            return this;
        }

        public IRequestRouterFluentBuilder UseRouteProvider(IRouteProvider routeProvider)
        {
            if (routeProvider == null)
            {
                throw new ArgumentNullException("routeProvider");
            }
            _routeProvider = routeProvider;
            return this;
        }
    }
}