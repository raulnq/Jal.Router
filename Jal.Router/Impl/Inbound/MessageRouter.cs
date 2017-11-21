using System.Linq;
using System.Reflection;
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

        public void Route<TContent>(InboundMessageContext<TContent> context, Route route)
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 2);

                var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod?.Invoke(this, new object[] { context, route });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
                else throw;
            }
        }

        public void Route<TContent, TData>(InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 3);

                var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType, typeof(TData));

                genericroutemethod?.Invoke(this, new object[] { context, route, data });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
                else throw;
            }
        }

        public void Execute<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route) where THandler : class
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

        public void Execute<TContent, THandler, TData>(InboundMessageContext<TContent> context, Route<TContent, THandler> route, TData data) where THandler : class
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