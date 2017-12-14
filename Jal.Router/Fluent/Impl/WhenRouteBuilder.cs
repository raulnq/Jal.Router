using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilder<TContent, TConsumer> : IWhenMethodBuilder<TContent, TConsumer>
    {
        private readonly RouteMethod<TContent, TConsumer> _routemethod;

        public WhenRouteBuilder(RouteMethod<TContent, TConsumer> routemethod)
        {

            if (routemethod == null)
            {
                throw new ArgumentNullException(nameof(routemethod));
            }

            _routemethod = routemethod;
        }

        public void When(Func<TContent, TConsumer, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.Evaluator = method;
        }

        public void When(Func<TContent, TConsumer, MessageContext, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.EvaluatorWithContext = method;
        }
    }
}