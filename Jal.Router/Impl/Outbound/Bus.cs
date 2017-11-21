using System;
using System.Collections.Generic;
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

        private void Send<TContent>(OutboundMessageContext<TContent> message, Options options)
        {
            var interceptor = _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);

            interceptor.OnEntry(message, options);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    var middlewares = new List<Type>();

                    middlewares.AddRange(_configuration.OutboundMiddlewareTypes);

                    middlewares.Add(typeof(PointToPointHandler));

                    var parameter = new MiddlewareParameter() { Options = options, OutboundType = "Send"};

                    var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), message, parameter);

                    pipeline.Execute();

                    interceptor.OnSuccess(message, options);
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

        public void Send<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                Saga = options.Saga,
                ContentType = typeof(TContent),
                DateTimeUtc = DateTime.UtcNow,
                Body = serializer.Serialize(content)
        };

            Send(message, options);
        }

        public void Send<TContent>(TContent content, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                var origin = endpoint.Origin;

                Send(content, setting, origin, options);
            }
        }

        public void Send<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                if (string.IsNullOrWhiteSpace(origin.Name))
                {
                    origin.Name = endpoint.Origin.Name;
                }

                if (string.IsNullOrWhiteSpace(origin.Key))
                {
                    origin.Key = endpoint.Origin.Key;
                }

                Send(content, setting, origin, options);
            }
        } 
        public void Publish<TContent>(TContent content, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                var origin = endpoint.Origin;

                Publish(content, setting, origin, options);
            }
        }

        public void Publish<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                if (string.IsNullOrWhiteSpace(origin.Name))
                {
                    origin.Name = endpoint.Origin.Name;
                }

                if (string.IsNullOrWhiteSpace(origin.Key))
                {
                    origin.Key = endpoint.Origin.Key;
                }

                Publish(content, setting, origin, options);
            }
        }

        public void Publish<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                Saga = options.Saga,
                ContentType = typeof(TContent),
                DateTimeUtc = DateTime.UtcNow,
                Body = serializer.Serialize(content)
            };

            Publish(message, options);
        }

        private void Publish<TContent>(OutboundMessageContext<TContent> message, Options options)
        {
            var interceptor = _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);

            interceptor.OnEntry(message, options);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    var middlewares = new List<Type>();

                    middlewares.AddRange(_configuration.OutboundMiddlewareTypes);

                    middlewares.Add(typeof(PublishSubscribeHandler));

                    var parameter = new MiddlewareParameter() { Options = options, OutboundType = "Publish" };

                    var pipeline = new Pipeline<TContent>(_factory, middlewares.ToArray(), message, parameter);

                    pipeline.Execute();

                    interceptor.OnSuccess(message, options);
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

        public void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                Origin = origin,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc,
                RetryCount = options.RetryCount,
                Saga = options.Saga,
                ContentType = typeof(TContent),
                DateTimeUtc = DateTime.UtcNow,
                Body = serializer.Serialize(content)
            };

            message.Origin.Key = string.Empty;

            Send(message, options);
        }

        public void FireAndForget<TContent>(TContent content, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                FireAndForget(content, setting, new Origin() {Key = endpoint.Origin.Key, Name = endpoint.Origin.Name }, options);
            }
        }

        public void FireAndForget<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoints = _provider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = _provider.Provide(endpoint, content);

                if (string.IsNullOrWhiteSpace(origin.Name))
                {
                    origin.Name = endpoint.Origin.Name;
                }

                if (string.IsNullOrWhiteSpace(origin.Key))
                {
                    origin.Key = endpoint.Origin.Key;
                }

                FireAndForget(content, setting, origin, options);
            }
        }
    }
}