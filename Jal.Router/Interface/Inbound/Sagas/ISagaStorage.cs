using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound.Sagas;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface ISagaStorage
    {
        void CreateMessage(MessageContext context, MessageEntity messageentity);

        string CreateSaga(MessageContext context, SagaEntity sagaentity);

        void UpdateSaga(MessageContext context, string id, SagaEntity sagaentity);

        void CreateMessage(MessageContext context, string id, SagaEntity sagaentity, MessageEntity messageentity);

        SagaEntity GetSaga(string id);

        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");

        MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "");

        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}