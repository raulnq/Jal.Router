using System;
using Jal.Router.Model.Inbound.Sagas;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface ISagaStorageSearcher
    {
        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");
        MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "");
        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}