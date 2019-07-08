using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractEntityStorage : IEntityStorage
    {
        public virtual Task CreateMessageEntity(MessageContext context, MessageEntity messageentity)
        {
            return Task.CompletedTask;
        }

        public virtual Task CreateSagaData(MessageContext context, SagaData sagadata)
        {
            return Task.CompletedTask;
        }

        public virtual Task UpdateSagaData(MessageContext context, SagaData sagadata)
        {
            return Task.CompletedTask;
        }

        public virtual Task<SagaData> GetSagaData(string id, Type sagatype)
        {
            return Task.FromResult(default(SagaData));
        }

        public virtual Task<SagaData[]> GetSagaData(DateTime start, DateTime end, Type sagatype, string saganame, string sagastoragename = "")
        {
            return Task.FromResult(default(SagaData[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, string messagestoragename = "")
        {
            return Task.FromResult(default(MessageEntity[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routenameorendpointname, string messagestoragename = "")
        {
            return Task.FromResult(default(MessageEntity[]));
        }
    }
}