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

        private const string DefaultStatus = "STARTED";

        public StartingMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var data = Activator.CreateInstance(parameter.Saga.DataType);

            var storage = _factory.Create<ISagaStorage>(_configuration.SagaStorageType);

            var serializer = _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);

            context.SagaContext.Status = DefaultStatus;

            var sagaentity = CreateSagaEntity(context, parameter);

            sagaentity.Data = serializer.Serialize(data);

            var id = storage.CreateSaga(context, sagaentity);

            context.SagaContext.Id = id;

            context.AddTrack(context.Identity.Id, context.Origin.Key, context.Origin.From, parameter.Route.Name, context.SagaContext.Id, parameter.Saga.Name);

            _router.Route(context, parameter.Route, data, parameter.Saga.DataType);

            sagaentity = storage.GetSaga(id);

            if (sagaentity != null)
            {
                sagaentity.Data = serializer.Serialize(data);

                sagaentity.Updated = context.DateTimeUtc;

                sagaentity.Status = context.SagaContext.Status;

                storage.UpdateSaga(context, id, sagaentity);

                var messageentity = CreateMessageEntity(context, parameter, sagaentity);

                try
                {
                    storage.CreateMessage(context, id, sagaentity, messageentity);
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
                throw new ApplicationException($"No data {parameter.Saga.DataType.FullName} for {parameter.Route.ContentType.FullName}, saga {parameter.Saga.Name}");
            }
        }
    }
}