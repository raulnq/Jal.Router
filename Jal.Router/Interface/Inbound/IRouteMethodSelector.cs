using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouteMethodSelector
    {
        bool Select<TContent, THandler>(MessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;
    }
}