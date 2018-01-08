using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class HandlerExecutor : IHandlerExecutor
    {
        public void Execute<TContent, THandler>(MessageContext context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext((TContent)context.Content, handler, context);
            }
            else
            {
                routemethod.Consumer?.Invoke((TContent)context.Content, handler);
            }
        }
        public void Execute<TContent, THandler, TData>(MessageContext context, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext((TContent)context.Content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    routemethod.Consumer((TContent)context.Content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        routemethod.ConsumerWithDataAndContext((TContent)context.Content, handler, context, data);
                    }
                    else
                    {
                        routemethod.ConsumerWithData?.Invoke((TContent)context.Content, handler, data);
                    }
                }
            }
        }
    }
}