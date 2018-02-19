using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorage
    {
        void CreateMessage(MessageContext context, MessageEntity entity);

        string CreateSaga(MessageContext context, SagaEntity entity);

        void UpdateSaga(MessageContext context, string id, SagaEntity entity);

        void CreateMessage(MessageContext context, string id, SagaEntity sagaentity, MessageEntity messageentity);

        SagaEntity GetSaga(string id);

        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");

        MessageEntity[] GetMessagesBySaga(SagaEntity entity, string messagestoragename = "");

        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}