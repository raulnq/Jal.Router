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
        
        public IBusInterceptor Interceptor { get; set; }

        public Bus(IEndPointProvider provider)
        {
            Provider = provider;

            Queue = AbstractQueue.Instance;

            Interceptor = AbstractBusInterceptor.Instance;
        }

        public void ReplyTo<TContent>(TContent content, InboundMessageContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var outboundmessagecontext = new OutboundMessageContext<TContent>
            {
                Id = context.Id,
                Content = content,
                //From = endpoint.From,
                ToConnectionString = context.ReplyToConnectionString,
                ToPath = context.ReplyToPath
            };

            var options = new Options() {Correlation = context.Id };

            try
            {
                Interceptor.OnEntry(outboundmessagecontext, options, "ReplyTo");

                if (!string.IsNullOrEmpty(outboundmessagecontext.ReplyToConnectionString) && !string.IsNullOrEmpty(outboundmessagecontext.ReplyToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnSuccess(outboundmessagecontext, options, "ReplyTo");
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(outboundmessagecontext, options, "ReplyTo", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(outboundmessagecontext, options, "ReplyTo",  stopwatch.ElapsedMilliseconds);
            }

        }


        public void Send<TContent>(TContent content, EndPointSetting endpoint, Options options)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var outboundmessagecontext = new OutboundMessageContext<TContent>
            {
                Id = options.MessageId,
                Content = content,
                From = endpoint.From,
                ReplyToConnectionString = endpoint.ReplyToConnectionString,
                ReplyToPath = endpoint.ReplyToPath,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath
            };

            Interceptor.OnEntry(outboundmessagecontext, options, "Send");

            try
            {
                if (!string.IsNullOrWhiteSpace(outboundmessagecontext.ToConnectionString) && !string.IsNullOrWhiteSpace(outboundmessagecontext.ToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnSuccess(outboundmessagecontext, options,"Send");
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(outboundmessagecontext, options, "Send", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(outboundmessagecontext, options, "Send", stopwatch.ElapsedMilliseconds);
            }

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
    }
}