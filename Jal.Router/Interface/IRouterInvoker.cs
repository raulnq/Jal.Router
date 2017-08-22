using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterInvoker
    {
        void Invoke<TContent>(InboundMessageContext<TContent> context, Route[] routes);

        void Invoke<TContent, TData>(InboundMessageContext<TContent> context, Route[] routes, TData data) where TData : class, new();
    }
}