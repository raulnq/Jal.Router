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

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext> method)
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

    public class WhitRouteBuilder<TBody, THandler, TData> : IWithMethodBuilder<TBody, THandler, TData>
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

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, TData> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Action<TBody, THandler, object> wrapper = (b, h, d) => method(b, h,(TData)d); 

            var routemethod = new RouteMethod<TBody, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TBody, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext, TData> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Action<TBody, THandler, MessageContext, object> wrapper = (b, h, i, d) => method(b, h, i,(TData)d);

            var routemethod = new RouteMethod<TBody, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, THandler>(routemethod);
        }
    }
}