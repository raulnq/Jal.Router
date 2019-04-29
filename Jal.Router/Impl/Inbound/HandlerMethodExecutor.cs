using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl.Inbound
{
    public class HandlerMethodExecutor : IHandlerMethodExecutor
    {
        public Task Execute<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                return routemethod.Consumer?.Invoke(content, handler);
            }
        }
        public Task Execute<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new()
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    return routemethod.Consumer(content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        return routemethod.ConsumerWithDataAndContext(content, handler, context, data);
                    }
                    else
                    {
                        return routemethod.ConsumerWithData?.Invoke(content, handler, data);
                    }
                }
            }
        }
    }
}