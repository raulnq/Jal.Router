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
                Correlation = context.Id,
                //From = endpoint.From,
                ToConnectionString = context.ReplyToConnectionString,
                ToPath = context.ReplyToPath
            };

            try
            {
                Interceptor.OnEntry(outboundmessagecontext, "ReplyTo");

                if (!string.IsNullOrEmpty(outboundmessagecontext.ReplyToConnectionString) && !string.IsNullOrEmpty(outboundmessagecontext.ReplyToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnSuccess(outboundmessagecontext, "ReplyTo");
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(outboundmessagecontext, "ReplyTo", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(outboundmessagecontext, "ReplyTo", stopwatch.ElapsedMilliseconds);
            }

        }


        public void Send<TContent>(TContent content, InboundMessageContext context, EndPointSetting endpoint, string id="")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var outboundmessagecontext = new OutboundMessageContext<TContent>
            {
                Id = id,
                Content = content,
                Correlation = context.Id,
                From = endpoint.From,
                ReplyToConnectionString = endpoint.ReplyToConnectionString,
                ReplyToPath = endpoint.ReplyToPath,
                ToConnectionString = endpoint.ToConnectionString,
                ToPath = endpoint.ToPath
            };

            Interceptor.OnEntry(outboundmessagecontext, "Send");

            try
            {
                if (!string.IsNullOrWhiteSpace(outboundmessagecontext.ToConnectionString) && !string.IsNullOrWhiteSpace(outboundmessagecontext.ToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnSuccess(outboundmessagecontext, "Send");
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnError(outboundmessagecontext, "Send", ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnExit(outboundmessagecontext, "Send", stopwatch.ElapsedMilliseconds);
            }

        }

        public void Send<TContent>(TContent content, InboundMessageContext context, string id="", string name = "")
        {
            var endpoints = Provider.Provide<TContent>(name);

            foreach (var endpoint in endpoints)
            {
                var setting = Provider.Provide(endpoint, content);

                Send(content, context, setting, id);
            }
        }
    }
}