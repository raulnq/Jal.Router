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
    public class MiddleMessageHandler : AbstractInboundMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly IMessageRouter _router;
        
        private const string DefaultStatus = "IN PROCESS";

        public MiddleMessageHandler(IComponentFactoryGateway factory, IMessageRouter router, IConfiguration configuration) : base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            context.Data.SagaContext.Status = DefaultStatus;

            var sagaentity = await GetSagaEntity(context.Data).ConfigureAwait(false);

            if (sagaentity != null)
            {
                context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

                await CreateMessageEntityAndSave(context.Data, sagaentity).ConfigureAwait(false);

                var serializer = Factory.CreateMessageSerializer();

                var data = serializer.Deserialize(sagaentity.Data, context.Data.Saga.DataType);

                if (data != null)
                {
                    await _router.Route(context.Data, data).ConfigureAwait(false);

                    sagaentity.Updated = context.Data.DateTimeUtc;

                    sagaentity.Data = serializer.Serialize(data);

                    await UpdateSagaEntity(context.Data, sagaentity).ConfigureAwait(false);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid saga record data {context.Data.Saga.DataType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No saga record type {context.Data.Saga.DataType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
            }
        }
    }
}