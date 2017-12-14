using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerExecutor
    {
        void Execute<TContent, THandler>(MessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        void Execute<TContent, THandler, TData>(MessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class;
    }
}