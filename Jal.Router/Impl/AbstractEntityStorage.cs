using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl
{

    public abstract class AbstractEntityStorage : IEntityStorage
    {
        public virtual Task<string> Insert(MessageEntity messageentity, IMessageSerializer serializer)
        {
            return Task.FromResult(string.Empty);
        }

        public virtual Task<string> Insert(SagaData sagadata, IMessageSerializer serializer)
        {
            return Task.FromResult(string.Empty);
        }

        public virtual Task Update(SagaData sagadata, IMessageSerializer serializer)
        {
            return Task.CompletedTask;
        }

        public virtual Task<SagaData> Get(string id, IMessageSerializer serializer)
        {
            return Task.FromResult(default(SagaData));
        }

        public virtual Task<SagaData[]> Get(DateTime start, DateTime end, string saganame, IMessageSerializer serializer, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(SagaData[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IMessageSerializer serializer, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(MessageEntity[]));
        }

        public virtual Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routenameorendpointname, IMessageSerializer serializer, IDictionary<string, string> options = null)
        {
            return Task.FromResult(default(MessageEntity[]));
        }
    }
}