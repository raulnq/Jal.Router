using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class StartingMessageHandler : AbstractMiddlewareMessageHandler, IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private readonly IConfiguration _configuration;

        public StartingMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var data = Activator.CreateInstance(parameter.Saga.DataType);

            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            context.SagaInfo.Status = "STARTED";

            var sagaentity = CreateSagaEntity(context, parameter);

            sagaentity.Data = serializer.Serialize(data);

            var id = storage.CreateSaga(context, sagaentity);

            context.SagaInfo.Id = id;

            context.AddTrack(context.Id, context.Origin.Key, context.Origin.From, parameter.Route.Name, context.SagaInfo.Id, parameter.Saga.Name);

            _router.Route(context, parameter.Route, data, parameter.Saga.DataType);

            sagaentity = storage.GetSaga(id);

            if (sagaentity != null)
            {
                sagaentity.Data = serializer.Serialize(data);

                sagaentity.Updated = context.DateTimeUtc;

                sagaentity.Status = context.SagaInfo.Status;

                storage.UpdateSaga(context, id, sagaentity);

                var messageentity = CreateMessageEntity(context, parameter, sagaentity);

                storage.CreateMessage(context, id, sagaentity, messageentity);
            }
        }
    }
}