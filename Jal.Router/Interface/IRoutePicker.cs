using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRoutePicker
    {
        bool Pick<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer) where THandler : class;
    }
}