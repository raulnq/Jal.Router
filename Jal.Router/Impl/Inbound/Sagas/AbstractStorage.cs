using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractStorage : IStorage
    {
        public virtual void Create(MessageContext context)
        {

        }

        public virtual void Create(MessageContext context, object data)
        {

        }

        public virtual void Update(MessageContext context, object data)
        {

        }

        public virtual object Find(MessageContext context)
        {
            return null;
        }
    }
}