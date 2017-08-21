using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IHandlerExecutor
    {
        void Execute<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer) where THandler : class;

        void Execute<TContent, THandler, TData>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer, TData data) where THandler : class;
    }
}