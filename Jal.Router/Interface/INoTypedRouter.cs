using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface INoTypedRouter
    {
        void Route<TContent>(InboundMessageContext<TContent> context, Route[] routes);

        void Route<TContent, TData>(InboundMessageContext<TContent> context, Route[] routes, TData data);
    }
}