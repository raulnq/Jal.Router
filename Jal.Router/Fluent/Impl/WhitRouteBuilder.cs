using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhitRouteBuilder<TContent, THandler> : IWithMethodBuilder<TContent, THandler>
    {
        private readonly Route<TContent, THandler> _route;

        public WhitRouteBuilder(Route<TContent, THandler> route)
        {
            _route = route;
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }
    }

    public class WhitRouteBuilder<TContent, THandler, TData> : IWithMethodBuilder<TContent, THandler, TData>
    {
        private readonly Route<TContent, THandler> _route;

        public WhitRouteBuilder(Route<TContent, THandler> route)
        {
            _route = route;
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, TData> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Action<TContent, THandler, object> wrapper = (b, h, d) => method(b, h,(TData)d); 

            var routemethod = new RouteMethod<TContent, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext, TData> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Action<TContent, THandler, MessageContext, object> wrapper = (b, h, i, d) => method(b, h, i,(TData)d);

            var routemethod = new RouteMethod<TContent, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }
    }
}