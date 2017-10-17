using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NextNameRouteBuilder<THandler, TData> : INextNameRouteBuilder<THandler, TData>
    {
        private readonly string _name;

        public List<Route> Routes { get; set; }

        private readonly Saga<TData> _saga;

        public NextNameRouteBuilder(Saga<TData> saga, string name)
        {
            _saga = saga;
            _name = name;
        }

        public IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>()
        {
            var value = new Route<TContent, THandler>(_name);

            var builder = new HandlerBuilder<TContent, THandler, TData>(value);

            _saga.NextRoutes.Add(value);

            return builder;
        }
    }
}