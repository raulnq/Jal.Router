using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{

    public class HandlerBuilder<TContent, THandler> : IHandlerBuilder<TContent, THandler>, IWhenHandlerBuilder, IOnRouteOptionBuilder
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

            _route.UpdateConsumerType(typeof (TConcreteConsumer));

            var whitRouteBuilder = new WhitRouteBuilder<TContent, THandler>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public void With(Action<IOnRouteWithBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteWithBuilder(_route);

            action(builder);
        }

        public IOnRouteOptionBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.UpdateWhen(condition);

            return this;
        }


        public IOnRouteOptionBuilder OnError(Action<IOnRouteErrorBuilder> action)
        {
            if (action==null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteErrorBuilder(_route);

            action(builder);

            return this;
        }

        public IOnRouteOptionBuilder OnExit(Action<IOnRouteExitBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteExitBuilder(_route);

            action(builder);

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

    public class HandlerBuilder<TContent, THandler, TData> : IHandlerBuilder<TContent, THandler, TData>, IWhenHandlerBuilder, IOnRouteOptionBuilder
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

            _route.UpdateConsumerType(typeof(TConcreteConsumer));

            var whitRouteBuilder = new WhitRouteBuilder<TContent, THandler, TData>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public IOnRouteOptionBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.UpdateWhen(condition);

            return this;
        }

        public IOnRouteOptionBuilder OnError(Action<IOnRouteErrorBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteErrorBuilder(_route);

            action(builder);

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

        public void With(Action<IOnRouteWithBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteWithBuilder(_route);

            action(builder);
        }

        public IOnRouteOptionBuilder OnExit(Action<IOnRouteExitBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnRouteExitBuilder(_route);

            action(builder);

            return this;
        }
    }
}