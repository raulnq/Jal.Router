using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerMethodExecutor
    {
        void Execute<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        void Execute<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new();
    }
}