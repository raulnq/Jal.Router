using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRoutePicker
    {
        bool Pick<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;
    }
}