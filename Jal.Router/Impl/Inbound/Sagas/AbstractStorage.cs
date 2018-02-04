using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractStorage : IStorage
    {
        public virtual void Create(MessageContext context)
        {

        }

        public virtual void StartSaga(MessageContext context, object data)
        {

        }

        public virtual void ContinueSaga(MessageContext context, object data)
        {

        }

        public virtual void UpdateSaga(MessageContext context, object data)
        {

        }

        public virtual void EndSaga(MessageContext context, object data)
        {

        }

        public virtual object FindSaga(MessageContext context)
        {
            return null;
        }
    }
}