using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Impl.ValueFinder;
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

        public IWhenHandlerBuilder Use<TConcreteConsumer>(Action<IWithMethodBuilder<TContent, THandler>> action) where TConcreteConsumer : THandler
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


        public IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo(string endpointname, Action<IForExceptionBuilder> action)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            if (action==null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new ForExceptionBuilder(_route.RetryExceptionTypes);

            action(builder);

            _route.OnRetryEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder Use<TValueFinder>(Func<IValueFinder, IRetryPolicy> policycreator) where TValueFinder : IValueFinder
        {
            _route.RetryValueFinderType = typeof(TValueFinder);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyProvider = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder Use(IRetryPolicy policy)
        {
            _route.RetryValueFinderType = typeof(NullValueFinder);

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Func<IValueFinder, IRetryPolicy> creator = x => policy;

            _route.RetryPolicyProvider = creator;

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

        public IWhenHandlerBuilder Use<TConcreteConsumer>(Action<IWithMethodBuilder<TContent, THandler, TData>> action) where TConcreteConsumer : THandler
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

        public IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo(string endpointname, Action<IForExceptionBuilder> action)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(nameof(endpointname));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new ForExceptionBuilder(_route.RetryExceptionTypes);

            action(builder);

            _route.OnRetryEndPoint = endpointname;

            return this;
        }

        public IOnRouteOptionBuilder Use<TValueFinder>(Func<IValueFinder, IRetryPolicy> policycreator) where TValueFinder : IValueFinder
        {
            _route.RetryValueFinderType = typeof(TValueFinder);

            if (policycreator == null)
            {
                throw new ArgumentNullException(nameof(policycreator));
            }

            _route.RetryPolicyProvider = policycreator;

            return this;
        }

        public IOnRouteOptionBuilder Use(IRetryPolicy policy)
        {
            _route.RetryValueFinderType = typeof(NullValueFinder);

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Func<IValueFinder, IRetryPolicy> creator = x => policy;

            _route.RetryPolicyProvider = creator;

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