using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class Bus : IBus
    {
        private readonly IEndPointProvider _provider;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public Bus(IEndPointProvider provider, IComponentFactory factory, IConfiguration configuration)
        {
            _provider = provider;
            _factory = factory;
            _configuration = configuration;
        }

        private TResult Reply<TResult>(MessageContext message, Options options)
        {
            var interceptor = _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var middlewares = new List<Type>
                    {
                        typeof(DistributionHandler)
                    };

                    middlewares.AddRange(_configuration.OutboundMiddlewareTypes);

                    middlewares.AddRange(message.EndPoint.MiddlewareTypes);

                    middlewares.Add(typeof(RequestReplyHandler));

                    var parameter = new MiddlewareParameter() { Options = options, OutboundType = "Reply", ResultType = typeof(TResult), Channel = message.EndPoint.Channels.First() };

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), message, parameter);

                    pipeline.Execute();

                    interceptor.OnSuccess(message, options);

                    return (TResult)parameter.Result;
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
        public TResult Reply<TContent, TResult>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            return Reply<TContent, TResult>(content, endpoint, endpoint.Origin, options);
        }
        public TResult Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var message = new MessageContext(endpoint)
            {
                Id = options.Id,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                SagaContext = options.SagaContext,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                ContentAsString = serializer.Serialize(content),
                ReplyToRequestId = options.ReplyToRequestId,
                RequestId = options.RequestId,
                ResultType = typeof(TResult),
                Tracks = options.Tracks
            };

            return Reply<TResult>(message, options);
        }
        public TResult Reply<TContent, TResult>(TContent content, Origin origin, Options options)
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
        private void Send(MessageContext message, Options options)
        {
            var interceptor = _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var middlewares = new List<Type>
                    {
                        typeof(DistributionHandler)
                    };

                    middlewares.AddRange(_configuration.OutboundMiddlewareTypes);

                    middlewares.AddRange(message.EndPoint.MiddlewareTypes);

                    middlewares.Add(typeof(PointToPointHandler));

                    var parameter = new MiddlewareParameter() {Options = options, OutboundType = "Send", Channel = message.EndPoint.Channels.First() };

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), message, parameter);

                    pipeline.Execute();
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

        public void Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var message = new MessageContext(endpoint)
            {
                Id = options.Id,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                SagaContext = options.SagaContext,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                ContentAsString = serializer.Serialize(content),
                ReplyToRequestId = options.ReplyToRequestId,
                RequestId = options.RequestId,
                Tracks = options.Tracks
            };

            Send(message, options);
        }

        public void Send<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            var origin = endpoint.Origin;

            Send(content, endpoint, origin, options);
        }
        public void Send<TContent>(TContent content, Origin origin, Options options)
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

            Send(content, endpoint, origin, options);
        } 
        public void Publish<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            var origin = endpoint.Origin;

            Publish(content, endpoint, origin, options);
        }
        public void Publish<TContent>(TContent content, Origin origin, Options options)
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

            Publish(content, endpoint, origin, options);
        }
        public void Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var message = new MessageContext(endpoint)
            {
                Id = options.Id,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                SagaContext = options.SagaContext,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                ContentAsString = serializer.Serialize(content),
                ReplyToRequestId = options.ReplyToRequestId,
                RequestId = options.RequestId,
                Tracks = options.Tracks
            };

            Publish(message, options);
        }

        private void Publish(MessageContext message, Options options)
        {
            var interceptor = _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);

            interceptor.OnEntry(message, options);

            try
            {
                if (message.EndPoint.Channels.Any())
                {
                    var middlewares = new List<Type>
                    {
                        typeof(DistributionHandler)
                    };

                    middlewares.AddRange(_configuration.OutboundMiddlewareTypes);

                    middlewares.AddRange(message.EndPoint.MiddlewareTypes);

                    middlewares.Add(typeof(PublishSubscribeHandler));

                    var parameter = new MiddlewareParameter() { Options = options, OutboundType = "Publish", Channel = message.EndPoint.Channels.First() };

                    var pipeline = new Pipeline(_factory, middlewares.ToArray(), message, parameter);

                    pipeline.Execute();
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

        public void FireAndForget<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var message = new MessageContext(endpoint)
            {
                Id = options.Id,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                SagaContext = options.SagaContext,
                ContentType = content.GetType(),
                DateTimeUtc = DateTime.UtcNow,
                ContentAsString = serializer.Serialize(content),
                Tracks = options.Tracks
            };

            message.Origin.Key = string.Empty;

            Send(message, options);
        }

        public void FireAndForget<TContent>(TContent content, Options options)
        {
            var endpoint = _provider.Provide(options.EndPointName, content.GetType());

            FireAndForget(content, endpoint, new Origin() { Key = endpoint.Origin.Key, From = endpoint.Origin.From }, options);
        }

        public void FireAndForget<TContent>(TContent content, Origin origin, Options options)
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

            FireAndForget(content, endpoint, origin, options);
        }
    }
}