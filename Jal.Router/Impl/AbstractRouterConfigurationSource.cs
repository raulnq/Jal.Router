using System.Collections.Generic;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public abstract class AbstractRouterConfigurationSource : IRouterConfigurationSource
    {
        private readonly List<Route> _routes = new List<Route>();

        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        public Route[] GetRoutes()
        {
            return _routes.ToArray();
        }

        public EndPoint[] GetEndPoints()
        {
            return _enpoints.ToArray();
        }

        public INameRouteBuilder<THandler> RegisterRoute<THandler>(string name="")
        {
            var builder = new NameRouteBuilder<THandler>(name, _routes);

            return builder;
        }


        public INameEndPointBuilder RegisterEndPoint<TExtractor>(string name = "") where TExtractor : IEndPointValueSettingFinder
        {
            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            var builder = new EndPointBuilder<TExtractor>(endpoint);

            return builder;
        }

        public void RegisterEndPoint<TExtractor, T>(string name = "") where TExtractor : IEndPointSettingFinder<T>
        {
            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            endpoint.ExtractorType = typeof(TExtractor);

            endpoint.MessageType = typeof(T);
        }
    }
}
