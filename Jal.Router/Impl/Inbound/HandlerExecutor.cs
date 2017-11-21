using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class HandlerExecutor : IHandlerExecutor
    {
        public void Execute<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext(context.Content, handler, context);
            }
            else
            {
                routemethod.Consumer?.Invoke(context.Content, handler);
            }
        }
        public void Execute<TContent, THandler, TData>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                routemethod.ConsumerWithContext(context.Content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    routemethod.Consumer(context.Content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        routemethod.ConsumerWithDataAndContext(context.Content, handler, context, data);
                    }
                    else
                    {
                        routemethod.ConsumerWithData?.Invoke(context.Content, handler, data);
                    }
                }
            }
        }
    }
}