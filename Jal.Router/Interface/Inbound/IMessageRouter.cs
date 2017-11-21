using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageRouter
    {
        void Route<TContent>(InboundMessageContext<TContent> context, Route route);

        void Route<TContent, TData>(InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new();
    }
}