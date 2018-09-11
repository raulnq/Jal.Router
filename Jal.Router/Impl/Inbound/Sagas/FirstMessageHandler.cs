using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class FirstMessageHandler : AbstractMessageHandler, IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private const string DefaultStatus = "STARTED";

        public FirstMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration):base(configuration)
        {
            _factory = factory;
            _router = router;
        }

        public void Execute(MessageContext messagecontext, Action<MessageContext, MiddlewareContext> next, MiddlewareContext middlewarecontext)
        {
            var data = Activator.CreateInstance(messagecontext.Saga.DataType);

            var storage = _factory.Create<ISagaStorage>(Configuration.SagaStorageType);

            var serializer = _factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            messagecontext.SagaContext.Status = DefaultStatus;

            var sagaentity = CreateSagaEntity(messagecontext);

            sagaentity.Data = serializer.Serialize(data);

            messagecontext.SagaContext.Id = storage.CreateSaga(messagecontext, sagaentity);

            messagecontext.AddTrack(messagecontext.Identity, messagecontext.Origin, messagecontext.Route, messagecontext.Saga, messagecontext.SagaContext);

            _router.Route(messagecontext, data);

            sagaentity.Updated = messagecontext.DateTimeUtc;

            SaveSaga(messagecontext, storage, sagaentity, serializer, data);

            SaveMessage(messagecontext, storage, sagaentity);
        }

    }
}