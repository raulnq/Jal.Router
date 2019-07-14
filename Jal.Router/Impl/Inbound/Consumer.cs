using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class Consumer : IConsumer
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public Consumer(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task Consume(MessageContext context)
        {
            try
            {
                if(context.SagaContext.SagaData?.Data==null)
                {
                    var routemethod = typeof(Consumer).GetMethods().First(x => x.Name == nameof(Consumer.Consume) && x.GetParameters().Count() == 2);

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType);

                    var task = genericroutemethod?.Invoke(this, new object[] { context, context.Route }) as Task;

                    await task;
                }
                else
                {
                    var routemethod = typeof(Consumer).GetMethods().First(x => x.Name == nameof(Consumer.Consume) && x.GetParameters().Count() == 3);

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType, context.Saga.DataType);

                    var task = genericroutemethod?.Invoke(this, new[] { context, context.Route, context.SagaContext.SagaData.Data }) as Task;

                    await task;
                }

            }
            catch (TargetInvocationException ex)
            {
                _logger.Log($"Exception during the handler execution {context.Route.ConsumerInterfaceType.FullName} route {context.Route.Name}");

                if (ex.InnerException != null) throw ex.InnerException;

                throw;
            }
        }

        public Task Consume<TContent, THandler>(MessageContext context, Route<TContent, THandler> route) where THandler : class
        {
            if (route.RouteMethods != null)
            {
                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    if (Select(context, content, method, consumer))
                    {
                        return Consume(context, content, method, consumer);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task Consume<TContent, THandler, TData>(MessageContext context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {

            if (route.RouteMethods != null)
            {
                var serializer = _factory.CreateMessageSerializer();

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                foreach (var method in route.RouteMethods)
                {
                    if (Select(context, content, method, consumer))
                    {
                        if (!string.IsNullOrWhiteSpace(method.Status))
                        {
                            context.SagaContext.SagaData.UpdateStatus(method.Status);
                        }

                        return Consume(context, content, method, consumer, data);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.EvaluatorWithContext == null)
            {
                if (routemethod.Evaluator == null)
                {
                    return true;
                }
                else
                {
                    if (routemethod.Evaluator(content, handler))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (routemethod.EvaluatorWithContext(content, handler, context))
                {
                    return true;
                }
            }

            return false;
        }

        public Task Consume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                return routemethod.Consumer?.Invoke(content, handler);
            }
        }

        public Task Consume<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new()
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    return routemethod.Consumer(content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        return routemethod.ConsumerWithDataAndContext(content, handler, context, data);
                    }
                    else
                    {
                        return routemethod.ConsumerWithData?.Invoke(content, handler, data);
                    }
                }
            }
        }
    }
}