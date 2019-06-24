using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorage
    {
        Task<MessageEntity> CreateMessageEntity(MessageContext context, MessageEntity messageentity);

        Task<SagaEntity> CreateSagaEntity(MessageContext context, SagaEntity sagaentity);

        Task UpdateSagaEntity(MessageContext context, SagaEntity sagaentity);

        Task<SagaEntity> GetSagaEntity(string id, Type sagatype);

        Task<SagaEntity[]> GetSagaEntities(DateTime start, DateTime end, string saganame, string sagastoragename = "");

        Task<MessageEntity[]> GetMessageEntitiesBySagaEntity(SagaEntity sagaentity, string messagestoragename = "");

        Task<MessageEntity[]> GetMessageEntities(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}