using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class HandlerBuilder : IHandlerBuilder, IWhenRouteBuilder, IOnRouteOptionBuilder, IForMessageRouteBuilder
    {
        private Route _route;

        public HandlerBuilder(Route route)
        {
            _route = route;
        }

        public IUseMethodBuilder<TContent> ForMessage<TContent>(Func<MessageContext, bool> condition = null)
        {
            var whitRouteBuilder = new UseMethodBuilder<TContent>(_route, condition);

            return whitRouteBuilder;
        }

        public IWhenRouteBuilder With(Action<IForMessageRouteBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(this);

            return this;
        }

        public IOnRouteOptionBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When(condition);

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

        public IOnRouteOptionBuilder UseMiddleware(Action<IRouteMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new RouteMiddlewareBuilder(_route);

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

    public class HandlerBuilder<TData> : IHandlerBuilder<TData>, IWhenRouteBuilder, IOnRouteOptionBuilder, IForMessageRouteBuilder<TData>
    {
        private Route _route;

        public HandlerBuilder(Route route)
        {
            _route = route;
        }

        public IOnRouteOptionBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When(condition);

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

        public IOnRouteOptionBuilder UseMiddleware(Action<IRouteMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new RouteMiddlewareBuilder(_route);

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

        public IWhenRouteBuilder With(Action<IForMessageRouteBuilder<TData>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(this);

            return this;
        }

        public IUseMethodBuilder<TContent, TData> ForMessage<TContent>(Func<MessageContext, bool> condition = null)
        {
            var whitRouteBuilder = new UseMethodBuilder<TContent, TData>(_route, condition);

            return whitRouteBuilder;
        }
    }
}