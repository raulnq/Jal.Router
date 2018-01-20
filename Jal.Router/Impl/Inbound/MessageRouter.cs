using System;
using System.Linq;
using System.Reflection;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

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

        public void Route(MessageContext context, Route route)
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 2);

                var genericroutemethod = routemethod?.MakeGenericMethod(route.ContentType, route.ConsumerInterfaceType);

                genericroutemethod?.Invoke(this, new object[] { context, route });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
                throw;
            }
        }

        public void Route(MessageContext context, Route route, object data, Type datatype)
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 3);

                var genericroutemethod = routemethod?.MakeGenericMethod(route.ContentType, route.ConsumerInterfaceType, datatype);

                genericroutemethod?.Invoke(this, new [] { context, route, data });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
                throw;
            }
        }

        public void Execute<TContent, THandler>(MessageContext context, Route<TContent, THandler> route) where THandler : class
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

        public void Execute<TContent, THandler, TData>(MessageContext context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
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
}