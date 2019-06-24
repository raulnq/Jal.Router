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
    public class MiddleConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _router;
        
        private const string DefaultStatus = "IN PROCESS";

        public MiddleConsumerMiddleware(IComponentFactoryGateway factory, IConsumer router, IConfiguration configuration) : base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            messagecontext.SagaContext.Status = DefaultStatus;

            messagecontext.SagaEntity = await GetSagaEntity(messagecontext).ConfigureAwait(false);

            if (messagecontext.SagaEntity != null)
            {
                context.Data.AddTrack(messagecontext.IdentityContext, messagecontext.Origin, messagecontext.Route, messagecontext.Saga, messagecontext.SagaContext);

                messagecontext.SagaContext.Data = messagecontext.SagaEntity.Data;

                if (messagecontext.SagaContext.Data != null)
                {
                    try
                    {
                        await _router.Consume(messagecontext).ConfigureAwait(false);

                        messagecontext.SagaEntity.Updated = messagecontext.DateTimeUtc;
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