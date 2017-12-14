using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageRouter
    {
        void Route<TContent>(MessageContext<TContent> context, Route route);

        void Route<TContent, TData>(MessageContext<TContent> context, Route route, TData data) where TData : class, new();
    }
}