using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class InitialMessageHandler : AbstractMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly IMessageRouter _router;

        private const string DefaultStatus = "STARTED";

        public InitialMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var data = Activator.CreateInstance(context.Data.Saga.DataType);

            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            context.Data.SagaContext.Status = DefaultStatus;

            var sagaentity = await CreateSagaEntity(context.Data).ConfigureAwait(false);

            sagaentity.Data = serializer.Serialize(data);

            context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

            await CreateMessageEntity(context.Data, MessageEntityType.Inbound, sagaentity).ConfigureAwait(false);

            await _router.Route(context.Data, data).ConfigureAwait(false);

            sagaentity.Data = serializer.Serialize(data);

            await UpdateSagaEntity(context.Data, sagaentity).ConfigureAwait(false);
        }
    }
}