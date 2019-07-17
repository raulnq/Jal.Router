using System;
using System.Threading.Tasks;
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

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method)
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

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, TData, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Func<TContent, THandler, object, Task> wrapper = (b, h, d) => method(b, h,(TData)d); 

            var routemethod = new RouteMethod<TContent, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, TData, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Func<TContent, THandler, MessageContext, object, Task> wrapper = (b, h, i, d) => method(b, h, i,(TData)d);

            var routemethod = new RouteMethod<TContent, THandler>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method, string status)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }
            var routemethod = new RouteMethod<TContent, THandler>(method, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, TData, Task> method, string status)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }
            Func<TContent, THandler, object, Task> wrapper = (b, h, d) => method(b, h, (TData)d);

            var routemethod = new RouteMethod<TContent, THandler>(wrapper, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method, string status)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }
            var routemethod = new RouteMethod<TContent, THandler>(method, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, TData, Task> method, string status)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }
            Func<TContent, THandler, MessageContext, object, Task> wrapper = (b, h, i, d) => method(b, h, i, (TData)d);

            var routemethod = new RouteMethod<TContent, THandler>(wrapper, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }
    }
}