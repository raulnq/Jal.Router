using System;
using System.Diagnostics;
using Jal.Router.Interface;
using Jal.Router.Model;
using System.Linq;

namespace Jal.Router.Impl
{
    public class Bus : IBus
    {
        public IEndPointProvider EnpointProvider { get; set; }

        public IRetryPolicyProvider RetryPolicyProvider { get; set; }

        public IQueue Queue { get; set; }

        public IPublisher Publisher { get; set; }

        public IBusInterceptor Interceptor { get; set; }

        public IBusLogger Logger { get; set; }

        public Bus(IEndPointProvider provider)
        {
            EnpointProvider = provider;

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

        public void Send<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options)
        {
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                From = origin.Name,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = origin.Key,
                Headers = options.Headers,
                Version = options.Version,
                ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc
            };

            Send(message, options);
        }

        public bool CanRetry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, IRetryPolicy retrypolicy)
        {
            var interval = retrypolicy.RetryInterval(inboundmessagecontext.RetryCount);

            return retrypolicy.CanRetry(inboundmessagecontext.RetryCount, interval);
        }

        public bool CanRetry<TContent>(TContent content, InboundMessageContext inboundmessagecontext)
        {
            var policy = RetryPolicyProvider.Provide<TContent>();

            return CanRetry(content, inboundmessagecontext, policy);
        }

        public void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint, IRetryPolicy retrypolicy)
        {
            var interval = retrypolicy.RetryInterval(inboundmessagecontext.RetryCount);

            var message = new OutboundMessageContext<TContent>
            {
                Id = inboundmessagecontext.Id,
                Content = content,
                From = inboundmessagecontext.From,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = inboundmessagecontext.Origin,
                Headers = inboundmessagecontext.Headers,
                Version = inboundmessagecontext.Version,
                ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(interval),
                RetryCount = inboundmessagecontext.RetryCount + 1,
            };

            var options = new Options()
            {
                Id = inboundmessagecontext.Id,
                Correlation = inboundmessagecontext.Id,
                Headers = inboundmessagecontext.Headers,
                ScheduledEnqueueDateTimeUtc = message.ScheduledEnqueueDateTimeUtc,
                Version = inboundmessagecontext.Version
            };

            Send(message, options);
        }

        public void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint)
        {
            var policy = RetryPolicyProvider.Provide<TContent>();

            Retry(content, inboundmessagecontext, endpoint, policy);
        }

        public void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext)
        {
            var endpoints = EnpointProvider.Provide<TContent>();

            var setting = EnpointProvider.Provide(endpoints.Single(), content);

            Retry(content, inboundmessagecontext, setting);
        }

        public void Send<TContent>(TContent content, Options options)
        {
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

                var origin = endpoint.Origin;

                Send(content, setting, origin, options);
            }
        }

        public void Send<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

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
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

                var origin = endpoint.Origin;

                Publish(content, setting, origin, options);
            }
        }

        public void Publish<TContent>(TContent content, Origin origin, Options options)
        {
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

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
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                From = origin.Name,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath,
                Origin = origin.Key,
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

        public void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options)
        {
            var message = new OutboundMessageContext<TContent>
            {
                Id = options.Id,
                Content = content,
                From = origin.Name,
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
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

                var origin = endpoint.Origin;

                FireAndForget(content, setting, origin, options);
            }
        }

        public void FireAndForget<TContent>(TContent content,Origin origin, Options options)
        {
            var endpoints = EnpointProvider.Provide<TContent>(options.EndPointName);

            foreach (var endpoint in endpoints)
            {
                var setting = EnpointProvider.Provide(endpoint, content);

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