using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        Task<string> Insert(MessageEntity messageentity, IMessageSerializer serializer);

        Task<string> Insert(SagaData sagadata, IMessageSerializer serializer);

        Task Update(SagaData sagadata, IMessageSerializer serializer);

        Task<SagaData> Get(string id, IMessageSerializer serializer);

        Task<SagaData[]> Get(DateTime start, DateTime end, string saganame, IMessageSerializer serializer, IDictionary<string,string> options=null);

        Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IMessageSerializer serializer, IDictionary<string, string> options = null);

        Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, IMessageSerializer serializer, IDictionary<string, string> options = null);
    }
}