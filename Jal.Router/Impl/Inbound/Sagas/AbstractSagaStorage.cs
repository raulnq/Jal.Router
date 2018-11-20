using System;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Model;
using Jal.Router.Model.Inbound.Sagas;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractSagaStorage : ISagaStorage
    {
        public virtual void CreateMessage(MessageContext context, MessageEntity messageentity)
        {

        }

        public virtual string CreateSaga(MessageContext context, SagaEntity sagaentity)
        {
            return string.Empty;
        }

        public virtual void CreateMessage(MessageContext context, string id, SagaEntity sagaentity, MessageEntity messageentity)
        {

        }

        public virtual void UpdateSaga(MessageContext context, string id, SagaEntity sagaentity)
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

        public virtual MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "")
        {
            return null;
        }

        public virtual MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            return null;
        }
    }
}