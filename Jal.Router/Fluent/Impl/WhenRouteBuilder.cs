using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilder<TBody, TConsumer> : IWhenMethodBuilder<TBody, TConsumer>, IRetryUsingBuilder
    {
        private readonly RouteMethod<TBody, TConsumer> _routemethod;

        public WhenRouteBuilder(RouteMethod<TBody, TConsumer> routemethod)
        {

            if (routemethod == null)
            {
                throw new ArgumentNullException(nameof(routemethod));
            }

            _routemethod = routemethod;
        }

        public IRetryBuilder When(Func<TBody, TConsumer, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.Evaluator = method;

            return this;
        }

        public IRetryBuilder When(Func<TBody, TConsumer, InboundMessageContext, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.EvaluatorWithContext = method;

            return this;
        }

        public IRetryUsingBuilder Retry<TExeption>() where TExeption : Exception
        {
            _routemethod.RetryExceptionType = typeof(TExeption);

            return this;
        }

        public void Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder
        {
            _routemethod.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _routemethod.RetryPolicyExtractor = policycreator;
        }
    }
}