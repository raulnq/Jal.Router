using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class HandlerExecutor : IHandlerExecutor
    {
        public void Execute<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer) where THandler : class
        {
            if (routeMethod.ConsumerWithContext != null)
            {
                routeMethod.ConsumerWithContext(context.Content, consumer, context);
            }
            else
            {
                routeMethod.Consumer?.Invoke(context.Content, consumer);
            }
        }
        public void Execute<TContent, THandler, TData>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer, TData data) where THandler : class
        {
            if (routeMethod.ConsumerWithContext != null)
            {
                routeMethod.ConsumerWithContext(context.Content, consumer, context);
            }
            else
            {
                if (routeMethod.Consumer != null)
                {
                    routeMethod.Consumer(context.Content, consumer);
                }
                else
                {
                    if (routeMethod.ConsumerWithDataAndContext != null)
                    {
                        routeMethod.ConsumerWithDataAndContext(context.Content, consumer, context, data);
                    }
                    else
                    {
                        routeMethod.ConsumerWithData?.Invoke(context.Content, consumer, data);
                    }
                }
            }
        }
    }
}