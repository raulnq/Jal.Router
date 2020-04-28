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

        private readonly ISenderContextLifecycle _lifecycle;

        private readonly ILogger _logger;

        public Bus(IEndPointProvider provider, IComponentFactoryFacade factory, IPipelineBuilder pipeline, ISenderContextLifecycle lifecycle, ILogger logger)
        {
            _provider = provider;
            _factory = factory;
            _pipeline = pipeline;
            _lifecycle = lifecycle;
            _logger = logger;
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options) where TResult : class
        {
            var endpoint = _provider.Provide(options, content.GetType());

            return Reply<TContent, TResult>(content, endpoint, endpoint.Origin, options);
        }

        public async Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options) where TResult: class
        {
            var result = await Dispatch(content, endpoint, origin, options).ConfigureAwait(false);

            return result as TResult;
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

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return Dispatch(content, endpoint, origin, options);
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
            return Dispatch(content, endpoint, origin, options);
        }

        private async Task<object> Dispatch(object content, EndPoint endpoint, Origin origin, Options options)
        {
            var interceptor = _factory.CreateBusInterceptor();

            var shuffler = _factory.CreateChannelShuffler();

            if (!endpoint.Channels.Any())
            {
                throw new ApplicationException($"Endpoint {endpoint.Name}, missing channels");
            }

            var channels = shuffler.Shuffle(endpoint.Channels.ToArray());

            var numberofchannels = channels.Length;

            var count = 0;

            foreach (var channel in channels)
            {
                count++;

                var sendercontext = _lifecycle.Get(channel);

                if (sendercontext == null)
                {
                    sendercontext = _lifecycle.Add(endpoint, channel);

                    sendercontext.Open();
                }

                var message = MessageContext.CreateToSend(sendercontext.MessageSerializer, sendercontext.EntityStorage, endpoint, channel, options, origin, content, DateTime.UtcNow);

                interceptor.OnEntry(message);

                try
                {
                    if (message.SagaContext.IsLoaded() && message.SagaContext.Data.IsValid())
                    {
                        message.SagaContext.Data.Update(message.DateTimeUtc);

                        await message.SagaContext.UpdateIntoStorage().ConfigureAwait(false);
                    }

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

                    interceptor.OnSuccess(message);

                    return message.ContentContext.ReplyData;
                }
                catch (Exception ex)
                {
                    interceptor.OnError(message, ex);

                    if (count < numberofchannels)
                    {
                        _logger.Log($"Message {message.Id} failed to distribute ({count}), moving to the next channel");
                    }
                    else
                    {
                        _logger.Log($"Message {message.Id} failed to distribute ({count}), no more channels");

                        throw;
                    }
                }
                finally
                {
                    interceptor.OnExit(message);
                }
            }

            return null;
        }
    }
}