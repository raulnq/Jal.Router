using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public class Consumer : IConsumer
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        private readonly ITypedConsumer _typedconsumer;

        public Consumer(IComponentFactoryFacade factory, ILogger logger, ITypedConsumer typedconsumer)
        {
            _factory = factory;
            _logger = logger;
            _typedconsumer = typedconsumer;
        }

        public Task Consume(MessageContext context)
        {
            if(!context.FromSaga())
            {
                return Consume(context, context.Route);
            }
            else
            {
                return Consume(context, context.Route, context.SagaContext.Data.Data);
            }
        }

        private async Task Consume(MessageContext context, Route route)
        {
            if (route.AnyRouteMethods())
            {
                foreach (var method in route.RouteMethods)
                {
                    var when = true;

                    if(method.Condition!=null)
                    {
                        when = method.Condition(context);
                    }

                    if(!when)
                    {
                        continue;
                    }

                    try
                    {
                        var content = context.MessageSerializer.Deserialize(context.ContentContext.Data, method.ContentType);

                        if (method.IsAnonymous)
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.AnonymousInnerConsume) && x.GetParameters().Count() == 3);

                            var genericroutemethod = routemethod?.MakeGenericMethod(method.ContentType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method }) as Task;

                            await task.ConfigureAwait(false);
                        }
                        else
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.InnerConsume) && x.GetParameters().Count() == 3);

                            var genericroutemethod = routemethod?.MakeGenericMethod(method.ContentType, method.HandlerType);

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
                await _typedconsumer.Consume(context, content, method, consumer).ConfigureAwait(false);
            }
        }

        private async Task AnonymousInnerConsume<TContent>(MessageContext context, TContent content, RouteMethod<TContent> method)
        {
            if (_typedconsumer.Select(context, content, method))
            {
                await _typedconsumer.Consume(context, content, method).ConfigureAwait(false);
            }
        }

        private async Task Consume(MessageContext context, Route route, object data)
        {

            if (route.AnyRouteMethods())
            {
                foreach (var method in route.RouteMethods)
                {
                    try
                    {
                        var when = true;

                        if (method.Condition != null)
                        {
                            when = method.Condition(context);
                        }

                        if (!when)
                        {
                            continue;
                        }

                        var content = context.MessageSerializer.Deserialize(context.ContentContext.Data, method.ContentType);

                        if (method.IsAnonymous)
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.AnonymousInnerConsume) && x.GetParameters().Count() == 4);

                            var genericroutemethod = routemethod?.MakeGenericMethod(method.ContentType, context.Saga.DataType);

                            var task = genericroutemethod?.Invoke(this, new object[] { context, content, method, data }) as Task;

                            await task.ConfigureAwait(false);
                        }
                        else
                        {
                            var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.InnerConsume) && x.GetParameters().Count() == 4);

                            var genericroutemethod = routemethod?.MakeGenericMethod(method.ContentType, context.Saga.DataType, method.HandlerType);

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

                await _typedconsumer.Consume(context, content, method, concretehandler, data).ConfigureAwait(false);
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

                await _typedconsumer.Consume(context, content, method, data).ConfigureAwait(false);
            }
        }
    }
}