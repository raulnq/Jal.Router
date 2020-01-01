using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        Task<string> Create(MessageEntity messageentity);

        Task<string> Create(SagaData sagadata);

        Task Update(SagaData sagadata);

        Task<SagaData> Get(string id);

        Task<SagaData[]> Get(DateTime start, DateTime end, string saganame, IDictionary<string,string> options=null);

        Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IDictionary<string, string> options = null);

        Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null);
    }
}