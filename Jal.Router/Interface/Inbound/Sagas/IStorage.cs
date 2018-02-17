using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

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

        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");

        MessageEntity[] GetMessagesBySaga(string sagakey, string messagestoragename = "");

        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}