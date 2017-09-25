using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class WhenRouteBuilder<TBody, TConsumer> : IWhenMethodBuilder<TBody, TConsumer>, IOnRetryUsingBuilder, IOnErrorBuilder
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

        public IOnRetryBuilder When(Func<TBody, TConsumer, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.Evaluator = method;

            return this;
        }

        public IOnRetryBuilder When(Func<TBody, TConsumer, InboundMessageContext, bool> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _routemethod.EvaluatorWithContext = method;

            return this;
        }

        public IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _routemethod.RetryExceptionType = typeof(TExeption);

            _routemethod.OnRetryEndPoint = endpointname;

            return this;
        }

        public IOnErrorBuilder Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder
        {
            _routemethod.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _routemethod.RetryPolicyExtractor = policycreator;

            return this;
        }

        public void OnErrorSendFailedMessageTo(string endpointname)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _routemethod.OnErrorEndPoint = endpointname;
        }
    }
}