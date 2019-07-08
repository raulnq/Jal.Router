using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorageGateway
    {
        Task<SagaData[]> GetSagas(DateTime start, DateTime end, Type sagatype, string saganame, string sagastoragename = "");
        Task<MessageEntity[]> GetMessagesBySaga(SagaData sagadata, string messagestoragename = "");
        Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}