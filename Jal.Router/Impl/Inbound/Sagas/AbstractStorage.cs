using System;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractStorage : IStorage
    {
        public virtual void CreateMessage(MessageContext context, MessageEntity entity)
        {

        }

        public virtual string CreateSaga(MessageContext context, SagaEntity entity)
        {
            return string.Empty;
        }

        public virtual void CreateMessage(MessageContext context, string id, SagaEntity sagaentity, MessageEntity messageentity)
        {

        }

        public virtual void UpdateSaga(MessageContext context, string id, SagaEntity entity)
        {
            
        }

        public virtual SagaEntity GetSaga(string id)
        {
            return null;
        }

        public virtual SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            return null;
        }

        public virtual MessageEntity[] GetMessagesBySaga(SagaEntity entity, string messagestoragename = "")
        {
            return null;
        }

        public virtual MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            return null;
        }
    }
}