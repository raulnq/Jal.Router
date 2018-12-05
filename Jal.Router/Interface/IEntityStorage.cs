using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        void CreateMessageEntity(MessageContext context, MessageEntity messageentity);

        void CreateSagaEntity(MessageContext context, SagaEntity sagaentity);

        void UpdateSagaEntity(MessageContext context, SagaEntity sagaentity);

        void CreateMessageEntity(MessageContext context, SagaEntity sagaentity, MessageEntity messageentity);

        SagaEntity GetSagaEntity(string entityid);

        SagaEntity[] GetSagaEntities(DateTime start, DateTime end, string saganame, string sagastoragename = "");

        MessageEntity[] GetMessageEntitiesBySagaEntity(SagaEntity sagaentity, string messagestoragename = "");

        MessageEntity[] GetMessageEntities(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}