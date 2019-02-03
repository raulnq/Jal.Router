using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEntityStorageFacade
    {
        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");
        MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "");
        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}