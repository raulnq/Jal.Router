using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class HandlerMethodExecutor : IHandlerMethodExecutor
    {
        public void Execute<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                routemethod.Consumer?.Invoke(content, handler);
            }
        }
        public void Execute<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new()
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    routemethod.Consumer(content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        routemethod.ConsumerWithDataAndContext(content, handler, context, data);
                    }
                    else
                    {
                        routemethod.ConsumerWithData?.Invoke(content, handler, data);
                    }
                }
            }
        }
    }
}