using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NoTypedRouter : INoTypedRouter
    {
        private readonly IRetryExecutor _retry;

        private readonly IHandlerFactory _factory;

        private readonly IRoutePicker _picker;

        private readonly IHandlerExecutor _executor;

        public NoTypedRouter(IHandlerFactory factory, IRoutePicker picker, IHandlerExecutor executor, IRetryExecutor retry)
        {
            _factory = factory;
            _picker = picker;
            _executor = executor;
            _retry = retry;
        }

        public void Route<TContent>(InboundMessageContext<TContent> context, Route[] routes)
        {
            var routemethod = GetType().GetMethods().First(x => x.Name == nameof(TypedRoute) && x.GetParameters().Count() == 2);

            foreach (var route in routes)
            {
                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod.Invoke(this, new object[] { context, route });
            }
        }

        public void Route<TContent, TData>(InboundMessageContext<TContent> context, Route[] routes, TData data)
        {
            var routemethod = GetType().GetMethods().First(x => x.Name == nameof(TypedRoute) && x.GetParameters().Count() == 3);

            foreach (var route in routes)
            {
                var genericroutemethod = routemethod.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType, typeof(TData));

                genericroutemethod.Invoke(this, new object[] { context, route, data });
            }
        }

        public void TypedRoute<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route) where THandler : class
        {
            if (CanApply(context, route))
            {
                var consumer = _factory.Create<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    _retry.Execute(() => {
                        if (_picker.Pick(context, method, consumer))
                        {
                            _executor.Execute(context, method, consumer);
                        }
                    }, method, context);                    
                }
            }
        }

        public void TypedRoute<TContent, THandler, TData>(InboundMessageContext<TContent> context, Route<TContent, THandler> route, TData data) where THandler : class
        {
            if (CanApply(context, route))
            {
                var consumer = _factory.Create<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    _retry.Execute(() => {
                        if (_picker.Pick(context, method, consumer))
                        {
                            _executor.Execute(context, method, consumer, data);
                        }
                    }, method, context);
                }
            }
        }

        private bool CanApply<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route)
            where THandler : class
        {
            var when = true;

            if (route.When != null)
            {
                when = route.When(context.Content, context);
            }
            return when;
        }
    }
}