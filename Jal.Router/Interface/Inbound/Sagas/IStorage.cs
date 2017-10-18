using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorage
    {
        void Create<TContent>(IndboundMessageContext<TContent> context, Route route);

        void Create<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route, TData data) where TData : class, new();

        void Update<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route, TData data) where TData : class, new();

        TData Find<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route) where TData : class, new();
    }
}