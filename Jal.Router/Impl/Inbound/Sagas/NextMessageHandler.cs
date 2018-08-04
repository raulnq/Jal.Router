using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class NextMessageHandler : AbstractMiddlewareMessageHandler, IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private readonly IConfiguration _configuration;

        private const string DefaultStatus = "IN PROCESS";

        public NextMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var storage = _factory.Create<ISagaStorage>(_configuration.SagaStorageType);

            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            var sagaentity = storage.GetSaga(context.SagaContext.Id);

            context.SagaContext.Status = DefaultStatus;

            if (sagaentity != null)
            {
                context.AddTrack(context.Identity.Id, context.Origin.Key, context.Origin.From, parameter.Route.Name, context.SagaContext.Id, parameter.Saga.Name);

                var data = serializer.Deserialize(sagaentity.Data, parameter.Saga.DataType);

                if (data != null)
                {
                    _router.Route(context, parameter.Route, data, parameter.Saga.DataType);

                    sagaentity.Data = serializer.Serialize(data);

                    sagaentity.Updated = context.DateTimeUtc;

                    sagaentity.Status = context.SagaContext.Status;

                    storage.UpdateSaga(context, context.SagaContext.Id, sagaentity);

                    var messageentity = CreateMessageEntity(context, parameter, sagaentity);

                    try
                    {
                        storage.CreateMessage(context, context.SagaContext.Id, sagaentity, messageentity);
                    }
                    catch (Exception)
                    {
                        if (!_configuration.Storage.IgnoreExceptionOnSaveMessage)
                        {
                            throw;
                        }
                    }
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