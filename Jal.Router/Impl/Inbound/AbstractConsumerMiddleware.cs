using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractConsumerMiddleware : AbstractMessageHandler
    {
        protected AbstractConsumerMiddleware(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }

        protected override MessageEntity MessageContextToMessageEntity(MessageContext context)
        {
            var entity = base.MessageContextToMessageEntity(context);

            entity.Type = MessageEntityType.Inbound;

            entity.ContentType = context.Route.ContentType.FullName;

            entity.Name = context.Route.Name;

            return entity;
        }

        protected Task<SagaEntity> GetSagaEntity(MessageContext messagecontext)
        {
            var storage = Factory.CreateEntityStorage();

            return storage.GetSagaEntity(messagecontext.SagaContext.Id, messagecontext.Saga.DataType);
        }

        protected Task UpdateSagaEntity(MessageContext messagecontext)
        {
            var storage = Factory.CreateEntityStorage();

            messagecontext.SagaEntity.Status = messagecontext.SagaContext.Status;

            return storage.UpdateSagaEntity(messagecontext, messagecontext.SagaEntity);
        }
    }
}