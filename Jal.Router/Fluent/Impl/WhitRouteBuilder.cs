using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhitRouteBuilder<TBody, THandler> : IWithMethodBuilder<TBody, THandler>
    {
        private readonly Route<TBody, THandler> _route;

        public WhitRouteBuilder(Route<TBody, THandler> route)
        {
            _route = route;
        }

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TBody, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, InboundMessageContext> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TBody, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, THandler>(routemethod);
        }
    }
}