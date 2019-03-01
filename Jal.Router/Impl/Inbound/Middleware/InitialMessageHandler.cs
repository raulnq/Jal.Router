using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class InitialMessageHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly IMessageRouter _router;

        private const string DefaultStatus = "STARTED";

        public InitialMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var data = Activator.CreateInstance(context.Data.Saga.DataType);

            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            context.Data.SagaContext.Status = DefaultStatus;

            var sagaentity = CreateSagaEntity(context.Data);

            sagaentity.Data = serializer.Serialize(data);

            context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

            CreateMessageEntity(context.Data, MessageEntityType.Inbound, sagaentity);

            _router.Route(context.Data, data);

            sagaentity.Data = serializer.Serialize(data);

            UpdateSagaEntity(context.Data, sagaentity);
        }
    }
}