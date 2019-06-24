using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractEntityStorage : IEntityStorage
    {
        public virtual Task<MessageEntity> CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {
            return Task.FromResult(default(MessageEntity));
        }

        public virtual Task<SagaEntity> CreateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            return Task.FromResult(default(SagaEntity));
        }

        public virtual Task UpdateSagaEntity(MessageContext context, SagaEntity sagaentity)
        {
            return Task.CompletedTask;
        }

        public virtual Task<SagaEntity> GetSagaEntity(string entityid, Type sagatype)
        {
            return Task.FromResult(default(SagaEntity));
        }

        public virtual Task<SagaEntity[]> GetSagaEntities(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            return Task.FromResult(default(SagaEntity[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntitiesBySagaEntity(SagaEntity sagaentity, string messagestoragename = "")
        {
            return Task.FromResult(default(MessageEntity[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routenameorendpointname, string messagestoragename = "")
        {
            return Task.FromResult(default(MessageEntity[]));
        }
    }
}