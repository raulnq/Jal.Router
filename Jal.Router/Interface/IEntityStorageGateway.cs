using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorageGateway
    {
        Task<SagaData[]> GetSagas(DateTime start, DateTime end, string saganame, IDictionary<string, string> options = null);
        Task<MessageEntity[]> GetMessagesBySaga(SagaData sagadata, IDictionary<string, string> options = null);
        Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null);
    }
}