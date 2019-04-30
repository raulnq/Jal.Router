using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly IHandlerMethodSelector _selector;

        private readonly IHandlerMethodExecutor _executor;

        public MessageRouter(IComponentFactoryGateway factory, IHandlerMethodSelector selector, IHandlerMethodExecutor executor)
        {
            _factory = factory;
            _selector = selector;
            _executor = executor;
        }

        public Task Route(MessageContext context)
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 2);

                var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType);

                var task = genericroutemethod?.Invoke(this, new object[] { context, context.Route });

                return task as Task;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw new Exception($"Exception during the handler execution {context.Route.ConsumerInterfaceType.FullName} route {context.Route.Name}",ex.InnerException);
                throw;
            }
        }

        public Task Route(MessageContext context, object data)
        {
            try
            {
                var routemethod = typeof(MessageRouter).GetMethods().First(x => x.Name == nameof(MessageRouter.Execute) && x.GetParameters().Count() == 3);

                var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType, context.Saga.DataType);

                var task = genericroutemethod?.Invoke(this, new [] { context, context.Route, data });

                return task as Task;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw new Exception($"Exception during the handler execution {context.Route.ConsumerInterfaceType.FullName} route {context.Route.Name} saga {context.Saga.Name}", ex.InnerException);
                throw;
            }
        }

        public Task Execute<TContent, THandler>(MessageContext context, Route<TContent, THandler> route) where THandler : class
        {
            if (route.RouteMethods != null)
            {
                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize<TContent>(context.Content);

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    if (_selector.Select(context, content, method, consumer))
                    {
                        return _executor.Execute(context, content, method, consumer);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task Execute<TContent, THandler, TData>(MessageContext context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {

            if (route.RouteMethods != null)
            {
                var serializer = _factory.CreateMessageSerializer();

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                var content = serializer.Deserialize<TContent>(context.Content);

                foreach (var method in route.RouteMethods)
                {
                    if (_selector.Select(context, content, method, consumer))
                    {
                        if (!string.IsNullOrWhiteSpace(method.Status))
                        {
                            context.SagaContext.Status = method.Status;
                        }

                        return _executor.Execute(context, content, method, consumer, data);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}