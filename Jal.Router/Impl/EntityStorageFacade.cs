using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class EntityStorageFacade : IEntityStorageFacade
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;
        public EntityStorageFacade(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            var storage = _factory.Create<IEntityStorage>(_configuration.StorageType);

            return storage.GetSagaEntities(start, end, saganame, sagastoragename);
        }

        public MessageEntity[] GetMessagesBySaga(SagaEntity sagaentity, string messagestoragename = "")
        {
            var storage = _factory.Create<IEntityStorage>(_configuration.StorageType);

            return storage.GetMessageEntitiesBySagaEntity(sagaentity, messagestoragename);
        }

        public MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            var storage = _factory.Create<IEntityStorage>(_configuration.StorageType);

            return storage.GetMessageEntities(start, end, routename, messagestoragename);
        }
    }
}