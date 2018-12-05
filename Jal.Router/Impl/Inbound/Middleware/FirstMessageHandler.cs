using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class FirstMessageHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly IMessageRouter _router;

        private const string DefaultStatus = "STARTED";

        public FirstMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var data = Activator.CreateInstance(context.Data.Saga.DataType);

            context.Data.SagaContext.Status = DefaultStatus;

            var sagaentity = CreateSagaEntity(context.Data, data);

            context.Data.SagaContext.Id = sagaentity.EntityId;

            context.Data.AddTrack(context.Data.Identity, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

            _router.Route(context.Data, data);

            sagaentity.Updated = context.Data.DateTimeUtc;

            UpdateSagaEntity(context.Data, sagaentity, data);
        }
    }
}