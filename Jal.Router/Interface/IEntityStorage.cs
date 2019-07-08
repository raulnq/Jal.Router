using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        Task CreateMessageEntity(MessageContext context, MessageEntity messageentity);

        Task CreateSagaData(MessageContext context, SagaData sagadata);

        Task UpdateSagaData(MessageContext context, SagaData sagadata);

        Task<SagaData> GetSagaData(string id, Type sagatype);

        Task<SagaData[]> GetSagaData(DateTime start, DateTime end, Type sagatype, string saganame, string sagastoragename = "");

        Task<MessageEntity[]> GetMessageEntitiesBySagaData(SagaData sagadata, string messagestoragename = "");

        Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}