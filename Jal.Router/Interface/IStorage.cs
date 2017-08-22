using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IStorage
    {
        void Create<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new();

        void Update<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new();

        TData Find<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new();
    }
}