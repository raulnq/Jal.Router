using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Bus : IBus
    {
        private readonly IEndPointProvider _provider;

        private readonly IComponentFactoryFacade _factory;

        private readonly IPipelineBuilder _pipeline;

        public Bus(IEndPointProvider provider, IComponentFactoryFacade factory, IPipelineBuilder pipeline)
        {
            _provider = provider;
            _factory = factory;
            _pipeline = pipeline;
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options) where TResult : class
        {
            var endpoint = _provider.Provide(options, content.GetType());

            return Reply<TContent, TResult>(content, endpoint, endpoint.Origin, options);
        }

        public async Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options) where TResult: class
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options, DateTime.UtcNow, origin, content.GetType(), serializer.Serialize(content));

            await Send(message).ConfigureAwait(false);

            return message.ContentContext.Result as TResult;
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options) where TResult : class
        {
            var endpoint = _provider.Provide(options, content.GetType());

            if (string.IsNullOrWhiteSpace(origin.From))
            {
                origin.From = endpoint.Origin.From;
            }

            if (string.IsNullOrWhiteSpace(origin.Key))
            {
                origin.Key = endpoint.Origin.Key;
            }

            return Reply<TContent, TResult>(content, endpoint, origin, options);
        }

        private async Task Update(MessageContext context)
        {
            if(context.SagaContext.Data!=null && context.SagaContext.Data.Data != null && !string.IsNullOrWhiteSpace(context.SagaContext.Data.Id))
            {
                context.SagaContext.Data.Update(context.DateTimeUtc);

                var storage = _factory.CreateEntityStorage();

                await storage.Update(context.SagaContext.Data).ConfigureAwait(false);
            }
        }

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options, DateTime.UtcNow, origin, content.GetType(), serializer.Serialize(content));

            return Send(message);
        }

        public Task Send<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options, content.GetType());

            return Send(content, endpoint, endpoint.Origin, options);
        }

        public Task Send<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options, content.GetType());

            if (string.IsNullOrWhiteSpace(origin.From))
            {
                origin.From = endpoint.Origin.From;
            }

            if (string.IsNullOrWhiteSpace(origin.Key))
            {
                origin.Key = endpoint.Origin.Key;
            }

            return Send(content, endpoint, origin, options);
        } 

        public Task Publish<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options, content.GetType());

            return Publish(content, endpoint, endpoint.Origin, options);
        }

        public Task Publish<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options, content.GetType());

            if (string.IsNullOrWhiteSpace(origin.From))
            {
                origin.From = endpoint.Origin.From;
            }

            if (string.IsNullOrWhiteSpace(origin.Key))
            {
                origin.Key = endpoint.Origin.Key;
            }

            return Publish(content, endpoint, origin, options);
        }

        public Task Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options, DateTime.UtcNow, origin, content.GetType(), serializer.Serialize(content));

            return Send(message);
        }

        private async Task Send(MessageContext message)
        {
            var interceptor = _factory.CreateBusInterceptor();

            interceptor.OnEntry(message);

            try
            {
                await Update(message).ConfigureAwait(false);

                if (message.EndPoint.Channels.Any())
                {
                    var chain = _pipeline.ForAsync<MessageContext>().UseAsync<BusMiddleware>();

                    foreach (var type in _factory.Configuration.OutboundMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in message.EndPoint.MiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync<ProducerMiddleware>().RunAsync(message).ConfigureAwait(false);
                }
                else
                {
                    throw new ApplicationException($"Endpoint {message.EndPoint.Name}, missing channels");
                }

                interceptor.OnSuccess(message);

            }
            catch (Exception ex)
            {
                interceptor.OnError(message, ex);

                throw;
            }
            finally
            {
                interceptor.OnExit(message);
            }
        }
    }
}