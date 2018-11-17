using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class HandlerBuilder<TContent, THandler> : IHandlerBuilder<TContent, THandler>, IWhenHandlerBuilder, IOnRetryUsingBuilder
    {
        private readonly Route<TContent, THandler> _route;

        public HandlerBuilder(Route<TContent, THandler> route)
        {
            _route = route;
        }

        public IWhenHandlerBuilder Using<TConcreteConsumer>(Action<IWithMethodBuilder<TContent, THandler>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof (TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TContent, THandler>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public IOnRetryBuilder When(Func<MessageContext, bool> condition)
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

        public IOnRouteOptionBuilder Using<TExtractor>(Func<IValueFinder, IRetryPolicy> policycreator) where TExtractor : IValueFinder
        {
            _route.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyExtractor = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder Using(IRetryPolicy policy)
        {
            _route.RetryExtractorType = typeof(NullValueSettingFinder);

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Func<IValueFinder, IRetryPolicy> creator = x => policy;

            _route.RetryPolicyExtractor = creator;

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

        public IOnRouteOptionBuilder UseMiddleware(Action<IInboundMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new InboundMiddlewareBuilder(_route);

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

        public IOnRouteOptionBuilder AsClaimCheck()
        {
            _route.UseClaimCheck = true;

            return this;
        }

        public IOnRouteOptionBuilder OnEntry(Action<IOnRouteEntryBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteEntryBuilder(_route);

            action(builder);

            return this;
        }
    }

    public class HandlerBuilder<TContent, THandler, TData> : IHandlerBuilder<TContent, THandler, TData>, IWhenHandlerBuilder, IOnRetryUsingBuilder
    {
        private readonly Route<TContent, THandler> _route;

        public HandlerBuilder(Route<TContent, THandler> route)
        {
            _route = route;
        }

        public IWhenHandlerBuilder Using<TConcreteConsumer>(Action<IWithMethodBuilder<TContent, THandler, TData>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof(TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TContent, THandler, TData>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public IOnRetryBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When = condition;

            return this;
        }

        public IOnRouteOptionBuilder AsClaimCheck()
        {
            _route.UseClaimCheck = true;

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

        public IOnRouteOptionBuilder Using<TExtractor>(Func<IValueFinder, IRetryPolicy> policycreator) where TExtractor : IValueFinder
        {
            _route.RetryExtractorType = typeof(TExtractor);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyExtractor = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder Using(IRetryPolicy policy)
        {
            _route.RetryExtractorType = typeof(NullValueSettingFinder);

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Func<IValueFinder, IRetryPolicy> creator = x => policy;

            _route.RetryPolicyExtractor = creator;

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

        public IOnRouteOptionBuilder UseMiddleware(Action<IInboundMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new InboundMiddlewareBuilder(_route);

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

        public IOnRouteOptionBuilder OnEntry(Action<IOnRouteEntryBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteEntryBuilder(_route);

            action(builder);

            return this;
        }
    }
}