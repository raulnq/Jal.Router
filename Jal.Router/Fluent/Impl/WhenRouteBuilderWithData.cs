using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilderWithData<TContent, THandler, TData> : IWhenMethodBuilderWithData<TContent, THandler, TData>
    {
        private readonly RouteMethodWithData<TContent, THandler, TData> _routemethod;

        public WhenRouteBuilderWithData(RouteMethodWithData<TContent, THandler, TData> routemethod)
        {
            _routemethod = routemethod;
        }

        public void When(Func<TContent, THandler, MessageContext, TData, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.When(method);
        }
    }

    public class WhenRouteBuilderWithData<TContent, TData> : IWhenMethodBuilderWithData<TContent, TData>
    {
        private readonly RouteMethodWithData<TContent, TData> _routemethod;

        public WhenRouteBuilderWithData(RouteMethodWithData<TContent, TData> routemethod)
        {
            _routemethod = routemethod;
        }

        public void When(Func<TContent, MessageContext, TData, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.When(method);
        }
    }
}