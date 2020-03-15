using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{

    public class HandlerBuilder<TContent> : IHandlerBuilder<TContent>, IWhenHandlerBuilder, IOnRouteOptionBuilder
    {
        private Route _route;

        private List<Route> _routes;

        private readonly string _name;

        private readonly List<Channel> _channels;

        public HandlerBuilder(List<Route> routes, string name, List<Channel> channels)
        {
            _routes = routes;
            _name = name;
            _channels = channels;
        }

        public IWhenHandlerBuilder Use(Action<IWithMethodBuilder<TContent>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var route = new Route(_name, typeof(TContent), _channels);

            _routes.Add(route);

            _route = route;

            var whitRouteBuilder = new WhitRouteBuilder<TContent>(route);

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

    public class HandlerBuilder<TContent, TData> : IHandlerBuilder<TContent, TData>, IWhenHandlerBuilder, IOnRouteOptionBuilder
    {
        private Route _route;

        private List<Route> _routes;

        private readonly string _name;

        private readonly List<Channel> _channels;

        public HandlerBuilder(List<Route> routes, string name, List<Channel> channels)
        {
            _routes = routes;
            _name = name;
            _channels = channels;
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

        public IWhenHandlerBuilder Use(Action<IWithMethodBuilder<TContent, TData>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var route = new Route(_name, typeof(TContent), _channels);

            _routes.Add(route);

            _route = route;

            var whitRouteBuilder = new WhitRouteBuilder<TContent, TData>(route);

            action(whitRouteBuilder);

            return this;
        }
    }
}