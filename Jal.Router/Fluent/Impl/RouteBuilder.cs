using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class RouteBuilder<TData> : IRouteBuilder<TData>
    {
        private readonly Saga _saga;

        private readonly List<Route> _routes;

        private readonly Type _middleware;

        public RouteBuilder(Saga saga, List<Route> routes, Type middleware)
        {
            _saga = saga;
            _routes = routes;
            _middleware = middleware;
        }

        public IListenerRouteBuilder<TData> RegisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var route = new Route(_saga, name, _middleware);

            _routes.Add(route);

            var builder = new ListenerRouteBuilder<TData>(route);

            return builder;
        }
    }
}