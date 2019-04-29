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
    public class FinalMessageHandler : AbstractMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly IMessageRouter _router;

        private const string DefaultStatus = "ENDED";

        public FinalMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration) : base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var sagaentity = await GetSagaEntity(context.Data).ConfigureAwait(false);

            context.Data.SagaContext.Status = DefaultStatus;

            if (sagaentity != null)
            {
                context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

                await CreateMessageEntity(context.Data, MessageEntityType.Inbound, sagaentity).ConfigureAwait(false);

                var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

                var data = serializer.Deserialize(sagaentity.Data, context.Data.Saga.DataType);

                if (data != null)
                {
                    await _router.Route(context.Data, data).ConfigureAwait(false);

                    sagaentity.Ended = context.Data.DateTimeUtc;

                    sagaentity.Duration = (sagaentity.Ended.Value - sagaentity.Created).TotalMilliseconds;

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