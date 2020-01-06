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

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType);

                    var task = genericroutemethod?.Invoke(this, new object[] { context, context.Route }) as Task;

                    await task;
                }
                else
                {
                    var routemethod = typeof(Consumer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(x => x.Name == nameof(Consumer.Consume) && x.GetParameters().Count() == 3);

                    var genericroutemethod = routemethod?.MakeGenericMethod(context.Route.ContentType, context.Route.ConsumerInterfaceType, context.Saga.DataType);

                    var task = genericroutemethod?.Invoke(this, new[] { context, context.Route, context.SagaContext.Data.Data }) as Task;

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

        private Task Consume<TContent, THandler>(MessageContext context, Route<TContent, THandler> route) where THandler : class
        {
            if (route.AnyRouteMethods())
            {
                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                foreach (var method in route.RouteMethods)
                {
                    if (_typedconsumer.Select(context, content, method, consumer))
                    {
                        return _typedconsumer.Consume(context, content, method, consumer);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task Consume<TContent, THandler, TData>(MessageContext context, Route<TContent, THandler> route, TData data) where THandler : class
            where TData : class, new()
        {

            if (route.AnyRouteMethods())
            {
                var serializer = _factory.CreateMessageSerializer();

                var consumer = _factory.CreateComponent<THandler>(route.ConsumerType);

                var content = serializer.Deserialize<TContent>(context.ContentContext.Data);

                foreach (var method in route.RouteMethods)
                {
                    if (_typedconsumer.Select(context, content, method, consumer))
                    {
                        if (!string.IsNullOrWhiteSpace(method.Status))
                        {
                            context.SagaContext.Data.SetStatus(method.Status);
                        }

                        return _typedconsumer.Consume(context, content, method, consumer, data);
                    }
                }
            }

            return Task.CompletedTask;
        }


    }
}