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
    public class FinalConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _router;

        private const string DefaultStatus = "ENDED";

        public FinalConsumerMiddleware(IComponentFactoryGateway factory, IConsumer router, IConfiguration configuration) : base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            messagecontext.SagaContext.Status = DefaultStatus;

            messagecontext.SagaEntity = await GetSagaEntity(context.Data).ConfigureAwait(false);

            if (messagecontext.SagaEntity != null)
            {
                context.Data.AddTrack(messagecontext.IdentityContext, messagecontext.Origin, messagecontext.Route, messagecontext.Saga, messagecontext.SagaContext);

                messagecontext.SagaContext.Data = messagecontext.SagaEntity.Data;

                if (context.Data.SagaContext.Data != null)
                {
                    try
                    {
                        await _router.Consume(context.Data).ConfigureAwait(false);

                        messagecontext.SagaEntity.Ended = messagecontext.DateTimeUtc;

                        messagecontext.SagaEntity.Duration = (messagecontext.SagaEntity.Ended.Value - messagecontext.SagaEntity.Created).TotalMilliseconds;

                    }
                    finally
                    {
                        await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
                    }

                    await UpdateSagaEntity(messagecontext).ConfigureAwait(false);
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