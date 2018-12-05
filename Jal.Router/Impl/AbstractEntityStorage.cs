using System;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractEntityStorage : IEntityStorage
    {
        public virtual void CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {

        }

        public virtual void CreateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {

        }

        public virtual void CreateMessageEntity(MessageContext context, SagaEntity sagaentity, MessageEntity messageentity)
        {

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