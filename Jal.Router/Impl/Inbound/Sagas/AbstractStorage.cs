using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractStorage : IStorage
    {
        public virtual void Create(MessageContext context)
        {

        }

        public virtual void Create<TData>(MessageContext context, TData data) where TData : class, new()
        {

        }

        public virtual void Update<TData>(MessageContext context,TData data) where TData : class, new()
        {

        }

        public virtual TData Find<TData>(MessageContext context) where TData : class, new()
        {
            return default(TData);
        }
    }
}