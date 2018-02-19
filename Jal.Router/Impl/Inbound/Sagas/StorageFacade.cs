using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class StorageFacade : IStorageFacade
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;
        public StorageFacade(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void UpdateSaga(MessageContext context, object data)
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var sagaentity = storage.GetSaga(context.SagaInfo.Id);

            if (sagaentity != null)
            {
                sagaentity.Data = serializer.Serialize(data);

                sagaentity.Updated = context.DateTimeUtc;

                sagaentity.Status = context.SagaInfo.Status;

                storage.UpdateSaga(context, context.SagaInfo.Id, sagaentity);
            }
        }

        public SagaEntity[] GetSagas(DateTime start, DateTime end, string saganame, string sagastoragename = "")
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            return storage.GetSagas(start, end, saganame, sagastoragename);
        }

        public MessageEntity[] GetMessagesBySaga(SagaEntity entity, string messagestoragename = "")
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            return storage.GetMessagesBySaga(entity, messagestoragename);
        }

        public MessageEntity[] GetMessages(DateTime start, DateTime end, string routename, string messagestoragename = "")
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            return storage.GetMessages(start, end, routename, messagestoragename);
        }
    }
}