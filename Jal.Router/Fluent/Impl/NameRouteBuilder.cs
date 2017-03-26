using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NameRouteBuilder<THandler> : INameRouteBuilder<THandler>
    {
        private readonly string _name;

        private readonly List<Route> _routes;

        public NameRouteBuilder(string name, List<Route> routes)
        {
            _name = name;

            _routes = routes;
        }

        public IHandlerBuilder<TBody, THandler> ForMessage<TBody>()
        {
            var value = new Route<TBody, THandler>(_name);

            var builder = new HandlerBuilder<TBody, THandler>(value);

            _routes.Add(value);

            return builder;
        }
    }
}