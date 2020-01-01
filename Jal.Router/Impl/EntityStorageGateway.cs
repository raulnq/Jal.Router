using System;
using System.Collections.Generic;
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

        public Task<SagaData[]> GetSagas(DateTime start, DateTime end, string saganame, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            return storage.Get(start, end, saganame, options);
        }

        public Task<MessageEntity[]> GetMessagesBySaga(SagaData sagadata, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            return storage.GetMessageEntitiesBySagaData(sagadata, options);
        }

        public Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            return storage.GetMessageEntities(start, end, routename, options);
        }
    }
}