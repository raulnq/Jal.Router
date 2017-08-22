using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ITypedRouter
    {
        void Route<TContent, THandler>(InboundMessageContext<TContent> context, Route<TContent, THandler> route) where THandler : class;

        void Route<TContent, THandler, TData>(InboundMessageContext<TContent> context, Route<TContent, THandler> route, TData data) where THandler : class, new();
    }
}