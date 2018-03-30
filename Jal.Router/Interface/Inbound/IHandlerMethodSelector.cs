using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerMethodSelector
    {
        bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;
    }
}