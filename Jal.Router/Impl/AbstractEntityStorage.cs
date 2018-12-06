using System;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractEntityStorage : IEntityStorage
    {
        public virtual MessageEntity CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {
            return null;
        }

        public virtual SagaEntity CreateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            return null;
        }

        public virtual MessageEntity CreateMessageEntity(MessageContext context, SagaEntity sagaentity, MessageEntity messageentity)
        {
            return null;
        }

        public virtual void UpdateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            
        }

        public virtual SagaEntity GetSagaEntity(string entityid)
        {
            return null;
        }

        public virtual SagaEntity[] GetSagaEntities(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            return null;
        }

        public virtual MessageEntity[] GetMessageEntitiesBySagaEntity(SagaEntity sagaentity, string messagestoragename = "")
        {
            return null;
        }

        public virtual MessageEntity[] GetMessageEntities(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            return null;
        }
    }
}