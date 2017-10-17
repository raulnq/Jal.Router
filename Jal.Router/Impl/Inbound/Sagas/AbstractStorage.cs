using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractStorage : IStorage
    {
        public virtual void Create<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {

        }

        public virtual void Update<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {

        }

        public virtual TData Find<TContent, TData>(Saga<TData> saga, IndboundMessageContext<TContent> context, Route route) where TData : class, new()
        {
            return default(TData);
        }
    }
}