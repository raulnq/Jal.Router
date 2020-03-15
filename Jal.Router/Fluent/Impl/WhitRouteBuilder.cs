using System;
using System.Threading.Tasks;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhitRouteBuilder<TContent> : IWithMethodBuilder<TContent>
    {
        private readonly Route _route;

        public WhitRouteBuilder(Route route)
        {
            _route = route;
        }

        public IWhenMethodBuilder<TContent, THandler> With<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, Task> method) where TConcreteHandler : THandler
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method, typeof(TConcreteHandler));

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent> With(Func<TContent, MessageContext, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent>(routemethod);
        }
    }

    public class WhitRouteBuilder<TContent, TData> : IWithMethodBuilder<TContent, TData>
    {
        private readonly Route _route;

        public WhitRouteBuilder(Route route)
        {
            _route = route;
        }

        public IWhenMethodBuilderWithData<TContent, THandler, TData> With<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, TData, Task> method, string status=null) where TConcreteHandler : THandler
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }

            var routemethod = new RouteMethodWithData<TContent, THandler, TData>(method, typeof(TConcreteHandler), status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilderWithData<TContent, THandler, TData>(routemethod);
        }

        public IWhenMethodBuilderWithData<TContent, TData> With(Func<TContent, MessageContext, TData, Task> method, string status = null)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }

            var routemethod = new RouteMethodWithData<TContent, TData>(method, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilderWithData<TContent, TData>(routemethod);
        }
    }
}