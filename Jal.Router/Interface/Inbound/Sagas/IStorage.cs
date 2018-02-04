using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorage
    {
        void Create(MessageContext context);

        void StartSaga(MessageContext context, object data);

        void ContinueSaga(MessageContext context, object data);

        void UpdateSaga(MessageContext context, object data);

        void EndSaga(MessageContext context, object data);

        object FindSaga(MessageContext context);
    }
}