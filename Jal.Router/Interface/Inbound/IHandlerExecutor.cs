using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerExecutor
    {
        void Execute<TContent, THandler>(IndboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        void Execute<TContent, THandler, TData>(IndboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class;
    }
}