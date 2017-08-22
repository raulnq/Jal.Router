using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class TypedRouter : ITypedRouter
    {
        private readonly IRetryExecutor _retry;

        private readonly IHandlerFactory _factory;

        private readonly IRoutePicker _picker;

        private readonly IHandlerExecutor _executor;

        public TypedRouter(IHandlerFactory factory, IRoutePicker picker, IHandlerExecutor executor, IRetryExecutor retry)
        {
            _factory = factory;
            _picker = picker;
            _executor = executor;
            _retry = retry;
        }

        public void Route<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route) where THandler : class
        {
            if (Can(context, route))
            {
                if (route.RouteMethods != null)
                {
                    var handler = _factory.Create<THandler>(route.ConsumerType);

                    foreach (var method in route.RouteMethods)
                    {
                        _retry.Execute(() => {
                            if (_picker.Pick(context, method, handler))
                            {
                                _executor.Execute(context, method, handler);
                            }
                        }, method, context);
                    }
                }
            }
        }

        public void Route<TContent, THandler, TData>(InboundMessageContext<TContent> context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {
            if (Can(context, route))
            {
                if (route.RouteMethods != null)
                {
                    var consumer = _factory.Create<THandler>(route.ConsumerType);

                    foreach (var method in route.RouteMethods)
                    {
                        _retry.Execute(() =>
                        {
                            if (_picker.Pick(context, method, consumer))
                            {
                                _executor.Execute(context, method, consumer, data);
                            }
                        }, method, context);
                    }
                }
            }
        }

        private bool Can<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route)
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