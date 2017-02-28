using System;
using System.Collections.Generic;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class MessageRouter : IMessageRouter
    {
        public MessageRouter(IMessageHandlerProvider provider)
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
            var handlers = Provider.Provide<TMessage>(message, route);

            if (handlers != null && handlers.Length > 0)
            {
                foreach (var handler in handlers)
                {
                    try
                    {
                        Interceptor.OnEntry(message, handler);

                        handler.Handle(message);

                        Interceptor.OnSuccess(message, handler);
                    }
                    catch (Exception ex)
                    {
                        Interceptor.OnError(message, handler, ex);
                        throw;
                    }
                    finally
                    {
                        Interceptor.OnExit(message, handler);
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
            var handlers = Provider.Provide<TMessage>(message, route);

            if (handlers != null && handlers.Length > 0)
            {
                foreach (var handler in handlers)
                {
                    try
                    {
                        Interceptor.OnEntry(message, handler);

                        handler.Handle(message, context);

                        Interceptor.OnSuccess(message, handler);
                    }
                    catch (Exception ex)
                    {
                        Interceptor.OnError(message, handler, ex);
                        throw;
                    }
                    finally
                    {
                        Interceptor.OnExit(message, handler);
                    }
                }
            }
            else
            {
                throw new Exception($"No routes were found for the message type {typeof(TMessage).FullName} and route {route}");
            }
        }

        public IMessageHandlerProvider Provider { get; set; }

        public IMessagetRouterInterceptor Interceptor { get; set; }
    }
}
