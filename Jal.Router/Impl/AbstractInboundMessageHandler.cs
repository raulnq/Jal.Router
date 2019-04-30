using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractInboundMessageHandler : AbstractMessageHandler
    {
        protected AbstractInboundMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }

        protected override MessageEntity MessageContextToMessageEntity(MessageContext context, SagaEntity sagaentity)
        {
            var entity = base.MessageContextToMessageEntity(context, sagaentity);

            entity.Type = MessageEntityType.Inbound;

            entity.ContentType = context.Route.ContentType.FullName;

            entity.Name = context.Route.Name;

            return entity;
        }

        protected Task<SagaEntity> GetSagaEntity(MessageContext messagecontext)
        {
            var storage = Factory.CreateEntityStorage();

            return storage.GetSagaEntity(messagecontext.SagaContext.Id);
        }

        protected async Task<SagaEntity> CreateSagaEntityAndSave(MessageContext context)
        {
            var storage = Factory.CreateEntityStorage();

            var sagaentity = MessageContextToSagaEntity(context);

            await storage.CreateSagaEntity(context, sagaentity).ConfigureAwait(false);

            context.SagaContext.Id = sagaentity.Id;

            return sagaentity;
        }

        protected Task UpdateSagaEntity(MessageContext messagecontext, SagaEntity sagaentity)
        {
            var storage = Factory.CreateEntityStorage();

            sagaentity.Status = messagecontext.SagaContext.Status;

            return storage.UpdateSagaEntity(messagecontext, sagaentity);
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
                Data = string.Empty
            };
        }
    }
}