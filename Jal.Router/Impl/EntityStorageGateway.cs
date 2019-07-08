using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class EntityStorageGateway : IEntityStorageGateway
    {
        private readonly IComponentFactoryGateway _factory;

        public EntityStorageGateway(IComponentFactoryGateway factory)
        {
            _factory = factory;
        }

        public Task<SagaData[]> GetSagas(DateTime start, DateTime end, Type sagatype, string saganame, string sagastoragename = "")
        {
            var storage = _factory.CreateEntityStorage();

            return storage.GetSagaData(start, end, sagatype, saganame, sagastoragename);
        }

        public Task<MessageEntity[]> GetMessagesBySaga(SagaData sagadata, string messagestoragename = "")
        {
            var storage = _factory.CreateEntityStorage();

            return storage.GetMessageEntitiesBySagaData(sagadata, messagestoragename);
        }

        public Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            var storage = _factory.CreateEntityStorage();

            return storage.GetMessageEntities(start, end, routename, messagestoragename);
        }
    }
}