using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IComponentFactory _factory;

        private readonly IRouteMethodSelector _selector;

        private readonly IHandlerExecutor _executor;

        public MessageRouter(IComponentFactory factory, IRouteMethodSelector selector, IHandlerExecutor executor)
        {
            _factory = factory;
            _selector = selector;
            _executor = executor;
        }

        public void Route<TContent>(IndboundMessageContext<TContent> context, Route route)
        {
            var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 2);

            var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

            genericroutemethod?.Invoke(this, new object[] { context, route });

        }

        public void Route<TContent, TData>(IndboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {
            var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 3);

            var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType, typeof(TData));

            genericroutemethod?.Invoke(this, new object[] { context, route, data });
        }

        public void Execute<TContent, THandler>(IndboundMessageContext<TContent> context, Route<TContent, THandler> route) where THandler : class
        {
            if (Can(context, route))
            {
                if (route.RouteMethods != null)
                {
                    var handler = _factory.Create<THandler>(route.ConsumerType);

                    foreach (var method in route.RouteMethods)
                    {
                        if (_selector.Select(context, method, handler))
                        {
                            _executor.Execute(context, method, handler);
                        }
                    }
                }
            }
        }

        public void Execute<TContent, THandler, TData>(IndboundMessageContext<TContent> context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {
            if (Can(context, route))
            {
                if (route.RouteMethods != null)
                {
                    var consumer = _factory.Create<THandler>(route.ConsumerType);
                    
                    foreach (var method in route.RouteMethods)
                    {
                        if (_selector.Select(context, method, consumer))
                        {
                            _executor.Execute(context, method, consumer, data);
                        }
                    }
                }
            }
        }

        private bool Can<TContent, THandler>(IndboundMessageContext<TContent> context, Route<TContent, THandler> route)
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