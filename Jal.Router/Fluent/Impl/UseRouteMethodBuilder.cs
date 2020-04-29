using System;
using System.Threading.Tasks;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class UseRouteMethodBuilder<TContent> : IUseMethodBuilder<TContent>
    {
        private readonly Route _route;

        private readonly Func<MessageContext, bool> _condition;

        public UseRouteMethodBuilder(Route route, Func<MessageContext, bool> condition)
        {
            _route = route;
            _condition = condition;
        }

        public IWhenMethodBuilder<TContent, THandler> Use<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, Task> method) where TConcreteHandler : THandler
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent, THandler>(method, typeof(TConcreteHandler), typeof(TContent), _condition);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent, THandler>(routemethod);
        }

        public IWhenMethodBuilder<TContent> Use(Func<TContent, MessageContext, Task> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TContent>(method, typeof(TContent), _condition);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TContent>(routemethod);
        }
    }

    public class UseRouteMethodBuilder<TContent, TData> : IUseMethodBuilder<TContent, TData>
    {
        private readonly Route _route;

        private readonly Func<MessageContext, bool> _condition;

        public UseRouteMethodBuilder(Route route, Func<MessageContext, bool> condition)
        {
            _route = route;
            _condition = condition;
        }

        public IWhenMethodBuilderWithData<TContent, THandler, TData> Use<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, TData, Task> method, string status=null) where TConcreteHandler : THandler
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }

            var routemethod = new RouteMethodWithData<TContent, THandler, TData>(method, typeof(TConcreteHandler), typeof(TContent), _condition, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilderWithData<TContent, THandler, TData>(routemethod);
        }

        public IWhenMethodBuilderWithData<TContent, TData> Use(Func<TContent, MessageContext, TData, Task> method, string status = null)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentNullException(nameof(status));
            }

            var routemethod = new RouteMethodWithData<TContent, TData>(method, typeof(TContent), _condition, status);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilderWithData<TContent, TData>(routemethod);
        }
    }
}