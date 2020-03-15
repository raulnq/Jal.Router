using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class Consumer : IConsumer
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        private readonly ITypedConsumer _typedconsumer;

        public Consumer(IComponentFactoryGateway factory, ILogger logger, ITypedConsumer typedconsumer)
        {
            _factory = factory;
            _logger = logger;
            _typedconsumer = typedconsumer;
        }

        public async Task Consume(MessageContext context)
        {
            try
            {
                if(!context.FromSaga())
                {
                    var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.Consume) && x.GetParameters().Count() == 2);

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType);

                    var task = genericroutemethod?.Invoke(this, new object[] { context, context.Route }) as Task;

                    await task.ConfigureAwait(false);
                }
                else
                {
                    var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.Consume) && x.GetParameters().Count() == 3);

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Saga.DataType);

                    var task = genericroutemethod?.Invoke(this, new[] { context, context.Route, context.SagaContext.Data.Data }) as Task;

                    await task.ConfigureAwait(false);
                }

            }
            catch (TargetInvocationException ex)
            {
                _logger.Log($"Exception during the route {context.Route.Name} execution");

                if (ex.InnerException != null) throw ex.InnerException;

                throw;
            }
        }

        private async Task Consume<TContent>(MessageContext context, Route route)
        {
            if (route.AnyRouteMethods())
            {
                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                foreach (var method in route.RouteMethods)
                {
                    try
                    {
                        if(method.IsAnonymous)
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.AnonymousInnerConsume) && x.GetParameters().Count() == 3);

                            var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method }) as Task;

                            await task.ConfigureAwait(false);
                        }
                        else
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.InnerConsume) && x.GetParameters().Count() == 3);

                            var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, method.HandlerType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method }) as Task;

                            await task.ConfigureAwait(false);
                        }


                    }
                    catch (TargetInvocationException ex)
                    {
                        _logger.Log($"Exception during the route method {method.HandlerType.FullName} execution");

                        if (ex.InnerException != null) throw ex.InnerException;

                        throw;
                    }
                }
            }
        }


        private async Task InnerConsume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> method) where THandler : class
        {
            var consumer = _factory.CreateComponent<THandler>(method.ConcreteHandlerType);

            if (_typedconsumer.Select(context, content, method, consumer))
            {
                await _typedconsumer.Consume(context, content, method, consumer);
            }
        }

        private async Task AnonymousInnerConsume<TContent>(MessageContext context, TContent content, RouteMethod<TContent> method)
        {
            if (_typedconsumer.Select(context, content, method))
            {
                await _typedconsumer.Consume(context, content, method);
            }
        }

        private async Task Consume<TContent, TData>(MessageContext context, Route route, TData data)
            where TData : class, new()
        {

            if (route.AnyRouteMethods())
            {
                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                foreach (var method in route.RouteMethods)
                {
                    try
                    {
                        if (method.IsAnonymous)
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.AnonymousInnerConsume) && x.GetParameters().Count() == 4);

                            var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Saga.DataType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method, data }) as Task;

                            await task.ConfigureAwait(false);
                        }
                        else
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.InnerConsume) && x.GetParameters().Count() == 4);

                            var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Saga.DataType, method.HandlerType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method, data }) as Task;

                            await task.ConfigureAwait(false);
                        }

                    }
                    catch (TargetInvocationException ex)
                    {
                        _logger.Log($"Exception during the route method {method.HandlerType.FullName} execution");

                        if (ex.InnerException != null) throw ex.InnerException;

                        throw;
                    }
                }
            }
        }

        private async Task InnerConsume<TContent, TData, THandler>(MessageContext context, TContent content, RouteMethodWithData<TContent, THandler, TData> method, TData data) where THandler : class
            where TData : class, new()
        {
            var concretehandler = _factory.CreateComponent<THandler>(method.ConcreteHandlerType);

            if (_typedconsumer.Select(context, content, method, concretehandler, data))
            {
                if (!string.IsNullOrWhiteSpace(method.Status))
                {
                    context.SagaContext.Data.SetStatus(method.Status);
                }

                await _typedconsumer.Consume(context, content, method, concretehandler, data);
            }
        }

        private async Task AnonymousInnerConsume<TContent, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, TData> method, TData data) where TData : class, new()
        {
            if (_typedconsumer.Select(context, content, method, data))
            {
                if (!string.IsNullOrWhiteSpace(method.Status))
                {
                    context.SagaContext.Data.SetStatus(method.Status);
                }

                await _typedconsumer.Consume(context, content, method, data);
            }
        }
    }
}