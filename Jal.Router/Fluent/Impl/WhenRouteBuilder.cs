using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilder<TBody, TConsumer> : IWhenMethodBuilder<TBody, TConsumer>
    {
        private readonly RouteMethod<TBody, TConsumer> _routemethod;

        public WhenRouteBuilder(RouteMethod<TBody, TConsumer> routemethod)
        {
            _routemethod = routemethod;
        }

        public void When(Func<TBody, TConsumer, bool> method)
        {
            _routemethod.Evaluator = method;
        }

        public void When<TContext>(Func<TBody, TConsumer, TContext, bool> method)
        {

            Func<TBody, TConsumer, dynamic, bool> wrapper = (b, c, d) => method(b, c, d);

            _routemethod.EvaluatorWithContext = wrapper;
        }
    }
}