using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class LastMessageHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private const string DefaultStatus = "ENDED";

        public LastMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration) : base(configuration)
        {
            _factory = factory;
            _router = router;
        }

        public void Execute(MessageContext messagecontext, Action<MessageContext, MiddlewareContext> next, MiddlewareContext middlewarecontext)
        {

        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var messagecontext = context.Data;

            var storage = _factory.Create<ISagaStorage>(Configuration.SagaStorageType);

            var serializer = _factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            var sagaentity = storage.GetSaga(messagecontext.SagaContext.Id);

            messagecontext.SagaContext.Status = DefaultStatus;

            if (sagaentity != null)
            {
                messagecontext.AddTrack(messagecontext.Identity, messagecontext.Origin, messagecontext.Route, messagecontext.Saga, messagecontext.SagaContext);

                var data = serializer.Deserialize(sagaentity.Data, messagecontext.Saga.DataType);

                if (data != null)
                {
                    _router.Route(messagecontext, data);

                    sagaentity.Ended = messagecontext.DateTimeUtc;

                    sagaentity.Duration = (sagaentity.Ended.Value - sagaentity.Created).TotalMilliseconds;

                    SaveSaga(messagecontext, storage, sagaentity, serializer, data);

                    SaveMessage(messagecontext, storage, sagaentity);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid data {messagecontext.Saga.DataType.FullName} for {messagecontext.Route.ContentType.FullName}, saga {messagecontext.Saga.Name} route {messagecontext.Route.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No data {messagecontext.Saga.DataType.FullName} for {messagecontext.Route.ContentType.FullName}, saga {messagecontext.Saga.Name} route {messagecontext.Route.Name}");
            }
        }
    }
}