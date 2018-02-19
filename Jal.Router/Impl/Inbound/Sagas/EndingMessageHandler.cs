using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class EndingMessageHandler : AbstractMiddlewareMessageHandler, IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private readonly IConfiguration _configuration;

        public EndingMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var sagaentity = storage.GetSaga(context.SagaInfo.Id);

            context.SagaInfo.Status = "ENDED";

            if (sagaentity != null)
            {
                var data = serializer.Deserialize(sagaentity.Data, parameter.Saga.DataType);

                context.AddTrack(context.Id, context.Origin.Key, context.Origin.From, parameter.Route.Name, context.SagaInfo.Id, parameter.Saga.Name);

                if (data != null)
                {
                    _router.Route(context, parameter.Route, data, parameter.Saga.DataType);

                    sagaentity.Data = serializer.Serialize(data);

                    sagaentity.Ended = context.DateTimeUtc;

                    sagaentity.Status = context.SagaInfo.Status;

                    sagaentity.Duration = (sagaentity.Ended.Value - sagaentity.Created).TotalMilliseconds;

                    storage.UpdateSaga(context, context.SagaInfo.Id, sagaentity);

                    var message = CreateMessageEntity(context, parameter, sagaentity);

                    storage.CreateMessage(context, context.SagaInfo.Id, sagaentity, message);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid data {parameter.Saga.DataType.FullName} for {parameter.Route.ContentType.FullName}, saga {parameter.Saga.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No data {parameter.Saga.DataType.FullName} for {parameter.Route.ContentType.FullName}, saga {parameter.Saga.Name}");
            }


        }
    }
}