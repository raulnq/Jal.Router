using System;
using System.Linq;
using System.Reflection;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IComponentFactory _factory;

        private readonly IHandlerMethodSelector _selector;

        private readonly IHandlerMethodExecutor _executor;

        private readonly IConfiguration _configuration;

        public MessageRouter(IComponentFactory factory, IHandlerMethodSelector selector, IHandlerMethodExecutor executor, IConfiguration configuration)
        {
            _factory = factory;
            _selector = selector;
            _executor = executor;
            _configuration = configuration;
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
                var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

                var handler = _factory.Create<THandler>(route.ConsumerType);

                var content = adapter.Deserialize<TContent>(context.Content);

                foreach (var method in route.RouteMethods)
                {
                    if (_selector.Select(context, content, method, handler))
                    {
                        _executor.Execute(context, content, method, handler);
                    }
                }
            }
        }

        public void Execute<TContent, THandler, TData>(MessageContext context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {

            if (route.RouteMethods != null)
            {
                var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

                var consumer = _factory.Create<THandler>(route.ConsumerType);

                var content = adapter.Deserialize<TContent>(context.Content);

                foreach (var method in route.RouteMethods)
                {
                    if (_selector.Select(context, content, method, consumer))
                    {
                        if (!string.IsNullOrWhiteSpace(method.Status))
                        {
                            context.SagaContext.Status = method.Status;
                        }

                        _executor.Execute(context, content, method, consumer, data);
                    }
                }
            }
        }
    }
}