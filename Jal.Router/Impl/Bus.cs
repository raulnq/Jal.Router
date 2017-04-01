using System;
using System.Diagnostics;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Bus : IBus
    {
        public IEndPointProvider Provider { get; set; }

        public IQueue Queue { get; set; }

        public IPublisher Publisher { get; set; }

        public IBusInterceptor Interceptor { get; set; }

        public Bus(IEndPointProvider provider)
        {
            Provider = provider;

            Queue = AbstractQueue.Instance;

            Interceptor = AbstractBusInterceptor.Instance;

            Publisher = AbstractPublisher.Instance;
        }

        private void Send<TContent>(OutboundMessageContext<TContent> message, Options options, string method)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Interceptor.OnEntry(message, options, method);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    Queue.Enqueue(message);

                    Interceptor.OnSuccess(message, options, method);
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(message, options, ex, method);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(message, options, stopwatch.ElapsedMilliseconds, method);
            }
        }

        public void Send<TContent>(TContent content, EndPointSetting endpoint, Options options)
        {
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.MessageId,
                Content = content,
                From = endpoint.From,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = endpoint.Origin
            };

            Send(message, options, "Send");
        }

        public void Send<TContent>(TContent content, Options options)
        {
            var endpoints = Provider.Provide<TContent>(options.EndPoint);

            foreach (var endpoint in endpoints)
            {
                var setting = Provider.Provide(endpoint, content);

                Send(content, setting, options);
            }
        }

        public void Publish<TContent>(TContent content, Options options)
        {
            var endpoints = Provider.Provide<TContent>(options.EndPoint);

            foreach (var endpoint in endpoints)
            {
                var setting = Provider.Provide(endpoint, content);

                Publish(content, setting, options);
            }
        }

        public void Publish<TContent>(TContent content, EndPointSetting endpoint, Options options)
        {
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.MessageId,
                Content = content,
                From = endpoint.From,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = options.Origin
            };

            Publish(message, options, "Publish");
        }

        private void Publish<TContent>(OutboundMessageContext<TContent> message, Options options, string method)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Interceptor.OnEntry(message, options, method);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    Publisher.Publish(message);

                    Interceptor.OnSuccess(message, options, method);
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(message, options, ex, method);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(message, options, stopwatch.ElapsedMilliseconds, method);
            }
        }

        public void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Options options)
        {
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.MessageId,
                Content = content,
                From = endpoint.From,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath
            };

            Send(message, options, "FireAndForget");
        }

        public void FireAndForget<TContent>(TContent content, Options options)
        {
            var endpoints = Provider.Provide<TContent>(options.EndPoint);

            foreach (var endpoint in endpoints)
            {
                var setting = Provider.Provide(endpoint, content);

                FireAndForget(content, setting, options);
            }
        }
    }
}