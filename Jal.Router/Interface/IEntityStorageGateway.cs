using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorageGateway
    {
        Task<SagaEntity[]> GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");
        Task<MessageEntity[]> GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "");
        Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}