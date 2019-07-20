using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl
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

        public virtual Task<SagaData> GetSagaData(string id)
        {
            return Task.FromResult(default(SagaData));
        }

        public virtual Task<SagaData[]> GetSagaData(DateTime start, DateTime end, string saganame, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(SagaData[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(MessageEntity[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routenameorendpointname, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(MessageEntity[]));
        }
    }
}