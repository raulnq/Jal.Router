using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NameRouteBuilder<TConsumer> : INameRouteBuilder<TConsumer>
    {
        private readonly string _name;

        private readonly List<Route> _routes;

        public NameRouteBuilder(string name, List<Route> routes)
        {
            _name = name;

            _routes = routes;
        }

        public IConcreteConsumerBuilder<TBody, TConsumer> ForMessage<TBody>()
        {
            var value = new Route<TBody, TConsumer>(_name);

            var builder = new ConcreteConsumerBuilder<TBody, TConsumer>(value);

            _routes.Add(value);

            return builder;
        }
    }
}