using System;
using System.Security.Cryptography.X509Certificates;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhitRouteBuilder<TBody, TConsumer> : IWhithMethodBuilder<TBody, TConsumer>
    {
        private readonly Route<TBody, TConsumer> _route;

        public WhitRouteBuilder(Route<TBody, TConsumer> route)
        {
            _route = route;
        }

        public IWhenMethodBuilder<TBody, TConsumer> With(Action<TBody, TConsumer> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var routemethod = new RouteMethod<TBody, TConsumer>(method);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, TConsumer>(routemethod);
        }

        public IWhenMethodBuilder<TBody, TConsumer> With<TContext>(Action<TBody, TConsumer, TContext> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Action<TBody, TConsumer, dynamic> wrapper = (b,c,d) => method(b,c,d);

            var routemethod = new RouteMethod<TBody, TConsumer>(wrapper);

            _route.RouteMethods.Add(routemethod);

            return new WhenRouteBuilder<TBody, TConsumer>(routemethod);
        }
    }
}