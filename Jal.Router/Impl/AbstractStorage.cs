using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractStorage : IStorage
    {
        public static IStorage Instance = new NullStorage();

        public virtual void Create<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {

        }

        public virtual void Update<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route, TData data) where TData : class, new()
        {

        }

        public virtual TData Find<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new()
        {
            return default(TData);
        }
    }
}