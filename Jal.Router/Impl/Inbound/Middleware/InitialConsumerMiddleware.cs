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
    public class InitialConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _router;

        private const string DefaultStatus = "STARTED";

        public InitialConsumerMiddleware(IComponentFactoryGateway factory, IConsumer router, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        private SagaEntity MessageContextToSagaEntity(MessageContext context)
        {
            return new SagaEntity
            {
                Created = context.DateTimeUtc,
                Updated = context.DateTimeUtc,
                Name = context.Saga.Name,
                DataType = context.Saga.DataType.FullName,
                Timeout = context.Saga.Timeout,
                Status = context.SagaContext.Status,
                Data = context.SagaContext.Data
            };
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            messagecontext.SagaContext.Status = DefaultStatus;

            messagecontext.SagaContext.Data = Activator.CreateInstance(messagecontext.Saga.DataType);

            var storage = Factory.CreateEntityStorage();

            messagecontext.SagaEntity = MessageContextToSagaEntity(messagecontext);

            await storage.CreateSagaEntity(messagecontext, messagecontext.SagaEntity).ConfigureAwait(false);

            messagecontext.SagaContext.Id = messagecontext.SagaEntity.Id;

            context.Data.AddTrack(messagecontext.IdentityContext, messagecontext.Origin, messagecontext.Route, messagecontext.Saga, messagecontext.SagaContext);

            try
            {
                await _router.Consume(messagecontext).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
            }

            await UpdateSagaEntity(messagecontext).ConfigureAwait(false);
        }
    }
}