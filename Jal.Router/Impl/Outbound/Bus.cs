using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.Router.Impl.Outbound.Middleware;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound
{
    public class Bus : IBus
    {
        private readonly IEndPointProvider _provider;

        private readonly IComponentFactoryGateway _factory;

        private readonly IConfiguration _configuration;

        private readonly IPipelineBuilder _pipeline;

        public Bus(IEndPointProvider provider, IComponentFactoryGateway factory, IConfiguration configuration, IPipelineBuilder pipeline)
        {
            _provider = provider;
            _factory = factory;
            _configuration = configuration;
            _pipeline = pipeline;
        }

        private async Task<TResult> Reply<TResult>(MessageContext message, Options options)
        {
            var interceptor = _factory.CreateBusInterceptor();

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var chain = _pipeline.ForAsync<MessageContext>().UseAsync<DistributionHandler>();

                    foreach (var type in _configuration.OutboundMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in message.EndPoint.MiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync<MessageHandler>().RunAsync(message).ConfigureAwait(false);

                    interceptor.OnSuccess(message, options);

                    return (TResult) message.Response;
                }
                else
                {
                    throw new ApplicationException($"Endpoint {message.EndPoint.Name}, missing channels");
                }
            }
            catch (Exception ex)
            {
                interceptor.OnError(message, options, ex);

                throw;
            }
            finally
            {
                interceptor.OnExit(message, options);
            }
        }
        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            return Reply<TContent, TResult>(content, endpoint, endpoint.Origin, options);
        }
        public Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options)
            {
                Origin = origin,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                Content = serializer.Serialize(content),
                Tracks = options.Tracks
            };

            return Reply<TResult>(message, options);
        }
        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

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
        private async Task Send(MessageContext message, Options options)
        {
            var interceptor = _factory.CreateBusInterceptor();

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var chain = _pipeline.ForAsync<MessageContext>().UseAsync<DistributionHandler>();

                    foreach (var type in _configuration.OutboundMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in message.EndPoint.MiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync<MessageHandler>().RunAsync(message).ConfigureAwait(false);
                }
                else
                {
                    throw new ApplicationException($"Endpoint {message.EndPoint.Name}, missing channels");
                }

                interceptor.OnSuccess(message, options);
            }
            catch (Exception ex)
            {
                interceptor.OnError(message, options, ex);

                throw;
            }
            finally
            {
                interceptor.OnExit(message, options);
            }
        }

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options)
            {
                Origin = origin,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                Content = serializer.Serialize(content),
                Tracks = options.Tracks
            };

            return Send(message, options);
        }

        public Task Send<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            var origin = endpoint.Origin;

            return Send(content, endpoint, origin, options);
        }
        public Task Send<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

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
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            var origin = endpoint.Origin;

            return Publish(content, endpoint, origin, options);
        }
        public Task Publish<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

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

            var message = new MessageContext(endpoint, options)
            {
                Origin = origin,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                Content = serializer.Serialize(content),
                Tracks = options.Tracks
            };

            return Publish(message, options);
        }

        private async Task Publish(MessageContext message, Options options)
        {
            var interceptor = _factory.CreateBusInterceptor();

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var chain = _pipeline.ForAsync<MessageContext>().UseAsync<DistributionHandler>();

                    foreach (var type in _configuration.OutboundMiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    foreach (var type in message.EndPoint.MiddlewareTypes)
                    {
                        chain.UseAsync(type);
                    }

                    await chain.UseAsync<MessageHandler>().RunAsync(message).ConfigureAwait(false);
                }
                else
                {
                    throw new ApplicationException($"Endpoint {message.EndPoint.Name}, missing channels");
                }

                interceptor.OnSuccess(message, options);

            }
            catch (Exception ex)
            {
                interceptor.OnError(message, options, ex);

                throw;
            }
            finally
            {
                interceptor.OnExit(message, options);
            }
        }

        public Task FireAndForget<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.CreateMessageSerializer();

            var message = new MessageContext(endpoint, options)
            {
                Origin = origin,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                Content = serializer.Serialize(content),
                Tracks = options.Tracks
            };

            message.Origin.Key = string.Empty;

            return Send(message, options);
        }

        public Task FireAndForget<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            return FireAndForget(content, endpoint, new Origin() { Key = endpoint.Origin.Key, From = endpoint.Origin.From }, options);
        }

        public Task FireAndForget<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            if (string.IsNullOrWhiteSpace(origin.From))
            {
                origin.From = endpoint.Origin.From;
            }

            if (string.IsNullOrWhiteSpace(origin.Key))
            {
                origin.Key = endpoint.Origin.Key;
            }

            return FireAndForget(content, endpoint, origin, options);
        }
    }
}