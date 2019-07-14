using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        Task CreateMessageEntity(MessageContext context, MessageEntity messageentity);

        Task CreateSagaData(MessageContext context, SagaData sagadata);

        Task UpdateSagaData(MessageContext context, SagaData sagadata);

        Task<SagaData> GetSagaData(string id);

        Task<SagaData[]> GetSagaData(DateTime start, DateTime end, string saganame, IDictionary<string,string> options=null);

        Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, IDictionary<string, string> options = null);

        Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null);
    }
}