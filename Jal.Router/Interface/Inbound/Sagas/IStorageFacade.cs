using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorageFacade
    {
        void UpdateSaga(MessageContext context, object data);
        SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "");
        MessageEntity[] GetMessagesBySaga(SagaEntity entity, string messagestoragename = "");
        MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "");
    }
}