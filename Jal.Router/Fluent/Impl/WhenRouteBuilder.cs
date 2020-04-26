using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilder<TContent, THandler> : IWhenMethodBuilder<TContent, THandler>
    {
        private readonly RouteMethod<TContent, THandler> _routemethod;

        public WhenRouteBuilder(RouteMethod<TContent, THandler> routemethod)
        {
            _routemethod = routemethod;
        }

        public void When(Func<TContent, THandler, MessageContext, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.When(method);
        }
    }

    public class WhenRouteBuilder<TContent> : IWhenMethodBuilder<TContent>
    {
        private readonly RouteMethod<TContent> _routemethod;

        public WhenRouteBuilder(RouteMethod<TContent> routemethod)
        {
            _routemethod = routemethod;
        }

        public void When(Func<TContent, MessageContext, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.When(method);
        }
    }
}