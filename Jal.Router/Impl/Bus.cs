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

        public void Reply<TContent>(TContent content, InboundMessageContext context)
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
                Interceptor.OnReplyEntry(outboundmessagecontext, options);

                if (!string.IsNullOrEmpty(outboundmessagecontext.ReplyToConnectionString) && !string.IsNullOrEmpty(outboundmessagecontext.ReplyToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnReplySuccess(outboundmessagecontext, options);
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnReplyError(outboundmessagecontext, options, ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnReplyExit(outboundmessagecontext, options, stopwatch.ElapsedMilliseconds);
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

            Interceptor.OnSendEntry(outboundmessagecontext, options);

            try
            {
                if (!string.IsNullOrWhiteSpace(outboundmessagecontext.ToConnectionString) && !string.IsNullOrWhiteSpace(outboundmessagecontext.ToPath))
                {
                    Queue.Enqueue(outboundmessagecontext);

                    Interceptor.OnSendSuccess(outboundmessagecontext, options);
                }
            }
            catch (Exception ex)
            {
                Interceptor.OnSendError(outboundmessagecontext, options, ex);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                Interceptor.OnSendExit(outboundmessagecontext, options,  stopwatch.ElapsedMilliseconds);
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