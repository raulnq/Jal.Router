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

        public IBusLogger Logger { get; set; }

        public Bus(IEndPointProvider provider)
        {
            Provider = provider;

            Queue = AbstractQueue.Instance;

            Interceptor = AbstractBusInterceptor.Instance;

            Publisher = AbstractPublisher.Instance;

            Logger = AbstractBusLogger.Instance;
        }

        private void Send<TContent>(OutboundMessageContext<TContent> message, Options options)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Logger.OnSendEntry(message, options);

            Interceptor.OnSendEntry(message, options);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    Queue.Enqueue(message);

                    Logger.OnSendSuccess(message, options);

                    Interceptor.OnSendSuccess(message, options);
                }
            }
            catch (Exception ex)
            {
                Logger.OnSendError(message, options, ex);

                Interceptor.OnSendError(message, options, ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Logger.OnSendExit(message, options, stopwatch.ElapsedMilliseconds);

                Interceptor.OnSendExit(message, options);
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
                Origin = endpoint.Origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc
            };

            Send(message, options);
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
                Origin = endpoint.Origin,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc
            };

            Publish(message, options);
        }

        private void Publish<TContent>(OutboundMessageContext<TContent> message, Options options)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Logger.OnPublishEntry(message, options);

            Interceptor.OnPublishEntry(message, options);

            try
            {
                if (!string.IsNullOrWhiteSpace(message.ToConnectionString) && !string.IsNullOrWhiteSpace(message.ToPath))
                {
                    Publisher.Publish(message);

                    Logger.OnPublishSuccess(message, options);

                    Interceptor.OnPublishSuccess(message, options);
                }
            }
            catch (Exception ex)
            {
                Logger.OnPublishError(message, options, ex);

                Interceptor.OnPublishError(message, options, ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Logger.OnPublishExit(message, options, stopwatch.ElapsedMilliseconds);

                Interceptor.OnPublishExit(message, options);
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
                ToPath = endpoint.ToPath,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc
            };

            Send(message, options);
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