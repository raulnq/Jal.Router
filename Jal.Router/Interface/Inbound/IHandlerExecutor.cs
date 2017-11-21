using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerExecutor
    {
        void Execute<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        void Execute<TContent, THandler, TData>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class;
    }
}