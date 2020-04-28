using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class EntityStorageFacade : IEntityStorageFacade
    {
        private readonly IComponentFactoryFacade _factory;

        public EntityStorageFacade(IComponentFactoryFacade factory)
        {
            _factory = factory;
        }

        public Task<SagaData[]> GetSagas(DateTime start, DateTime end, string saganame, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            var serializer = _factory.CreateMessageSerializer();

            return storage.Get(start, end, saganame, serializer, options);
        }

        public Task<MessageEntity[]> GetMessagesBySaga(SagaData sagadata, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            var serializer = _factory.CreateMessageSerializer();

            return storage.GetMessageEntitiesBySagaData(sagadata, serializer, options);
        }

        public Task<MessageEntity[]> GetMessages(DateTime start, DateTime end, string routename, IDictionary<string, string> options = null)
        {
            var storage = _factory.CreateEntityStorage();

            var serializer = _factory.CreateMessageSerializer();

            return storage.GetMessageEntities(start, end, routename, serializer, options);
        }
    }
}