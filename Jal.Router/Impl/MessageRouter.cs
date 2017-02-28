using System;
using System.Collections.Generic;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class MessageRouter : IMessageRouter
    {
        public MessageRouter(IMessageSenderProvider provider)
        {
            Provider = provider;

            Interceptor = AbstractMessagetRouterInterceptor.Instance;
        }

        public void Route<TMessage>(TMessage message)
        {
            Route<TMessage>(message, string.Empty);
        }

        public void Route<TMessage>(TMessage message, string route)
        {
            var senders = Provider.Provide<TMessage>(message, route);

            if (senders != null && senders.Length > 0)
            {
                foreach (var r in senders)
                {
                    try
                    {
                        Interceptor.OnEntry(message, r);

                        r.Send(message);

                        Interceptor.OnSuccess(message, r);
                    }
                    catch (Exception ex)
                    {
                        Interceptor.OnError(message, r, ex);
                        throw;
                    }
                    finally
                    {
                        Interceptor.OnExit(message, r);
                    }
                }
            }
            else
            {
                throw new Exception($"No routes were found for the message type {typeof (TMessage).FullName} and route {route}");
            }
        }

        public void Route<TMessage>(TMessage message, dynamic context)
        {
            Route<TMessage>(message, context, string.Empty);
        }

        public void Route<TMessage>(TMessage message, dynamic context, string route)
        {
            var senders = Provider.Provide<TMessage>(message, route);

            if (senders != null && senders.Length > 0)
            {
                foreach (var r in senders)
                {
                    try
                    {
                        Interceptor.OnEntry(message, r);

                        r.Send(message, context);

                        Interceptor.OnSuccess(message, r);
                    }
                    catch (Exception ex)
                    {
                        Interceptor.OnError(message, r, ex);
                        throw;
                    }
                    finally
                    {
                        Interceptor.OnExit(message, r);
                    }
                }
            }
            else
            {
                throw new Exception($"No routes were found for the message type {typeof(TMessage).FullName} and route {route}");
            }
        }

        public IMessageSenderProvider Provider { get; set; }

        public IMessagetRouterInterceptor Interceptor { get; set; }
    }
}
