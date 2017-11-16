using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class HandlerBuilder<TBody, THandler> : IHandlerBuilder<TBody, THandler>, IWhenHandlerBuilder<TBody>, IOnRetryUsingBuilder
    {
        private readonly Route<TBody, THandler> _route;

        public HandlerBuilder(Route<TBody, THandler> route)
        {
            _route = route;
        }

        public IWhenHandlerBuilder<TBody> ToBeHandledBy<TConcreteConsumer>(Action<IWithMethodBuilder<TBody, THandler>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof (TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TBody, THandler>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public IOnRetryBuilder When(Func<TBody, MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When = condition;

            return this;
        }


        public IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.RetryExceptionType = typeof(TExeption);

            _route.OnRetryEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder
        {
            _route.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyExtractor = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder OnErrorSendFailedMessageTo(string endpointname)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.OnErrorEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder UsingMiddleware(Action<IMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new MiddlewareBuilder(_route);

            action(builder);

            return this;
        }

        public IOnRouteOptionBuilder ForwardMessageTo(string endpointname)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.ForwardEndPoint = endpointname;

            return this;
        }
    }

    public class HandlerBuilder<TBody, THandler, TData> : IHandlerBuilder<TBody, THandler, TData>, IWhenHandlerBuilder<TBody>, IOnRetryUsingBuilder
    {
        private readonly Route<TBody, THandler> _route;

        public HandlerBuilder(Route<TBody, THandler> route)
        {
            _route = route;
        }

        public IWhenHandlerBuilder<TBody> ToBeHandledBy<TConcreteConsumer>(Action<IWithMethodBuilder<TBody, THandler, TData>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof(TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TBody, THandler, TData>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public IOnRetryBuilder When(Func<TBody, MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When = condition;

            return this;
        }


        public IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.RetryExceptionType = typeof(TExeption);

            _route.OnRetryEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder
        {
            _route.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyExtractor = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder OnErrorSendFailedMessageTo(string endpointname)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.OnErrorEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder UsingMiddleware(Action<IMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new MiddlewareBuilder(_route);

            action(builder);

            return this;
        }

        public IOnRouteOptionBuilder ForwardMessageTo(string endpointname)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            _route.ForwardEndPoint = endpointname;

            return this;
        }
    }
}